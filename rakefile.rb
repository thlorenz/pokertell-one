# possible MSBuild verbosities: q[uiet], m[inimal], n[ormal], d[etailed], and diag[nostic]

SOLUTIONPATH = "PokerTell.sln"

DOTNETVERSION = "4.0.30319"
DOTNETVERSIONMAJOR = "4.0"

MSBUILD = File.join(ENV["windir"] || ENV["WINDIR"], "Microsoft.Net", "Framework","v#{DOTNETVERSION}", "msbuild.exe")

CONFIG = ENV["config"] || ENV["CONFIG"] || "Debug"
MSBUILD_VERBOSITY_LEVEL = ENV["verbosity"] || "normal" 

CURRENT_PATH = File.dirname(__FILE__)
REPORT_PATH = File.join(CURRENT_PATH, "specs.reports")
TOOLS_PATH = File.join("c:/dev/", "tools")
TEST_SUFFIX = "Tests"

output_teamcity_report = false
output_silent = true
output_html_report = false

task :init_html_reportpath do 
  FileUtils.remove_dir(REPORT_PATH, true)
  FileUtils.mkdir_p REPORT_PATH if output_html_report
end

desc "Cleans the build"
task :clean do
  msclean_succeeded = system "\"#{MSBUILD}\" \"#{SOLUTIONPATH}\" /p:Configuration=#{CONFIG} /t:Clean /tv:#{DOTNETVERSIONMAJOR}"

  raise "Failed to clean \"#{SOLUTIONPATH}\"" unless msclean_succeeded
end

desc "Build #{SOLUTIONPATH}"
task :build do
  msbuild_succeeded = system "\"#{MSBUILD}\" \"#{SOLUTIONPATH}\" /p:Configuration=#{CONFIG} /t:Build /tv:#{DOTNETVERSIONMAJOR} /verbosity:#{MSBUILD_VERBOSITY_LEVEL}"

  raise "Failed to build \"#{SOLUTIONPATH}\"" unless msbuild_succeeded
end

desc "Run Specs"
task :run_specs do
  spec_dlls = Dir.glob(File.join(CURRENT_PATH,"Src/Tests/UnitTests/**/bin/#{CONFIG}/*.Tests.dll")).map { |dll| dll + " " }

	mspec_runner_exe = File.join(TOOLS_PATH, "mspec", "mspec.exe")
  teamcity_flag = output_teamcity_report ? '--teamcity' : '' 
  console_silent_flag = output_silent ? '--silent' : ''
  html_report_flag = output_html_report ? "--html \"#{REPORT_PATH}\" --timeinfo" : ''  
	mspec_runner_argument = "\"#{mspec_runner_exe}\" #{teamcity_flag} #{console_silent_flag} #{html_report_flag} #{spec_dlls}"

  nunit_runner_exe = File.join(TOOLS_PATH,"NUnit/bin/net-2.0", "nunit-console.exe")
  console_silent_flag = output_silent ? '' : '/labels'
  nunit_runner_argument = "\"#{nunit_runner_exe}\" #{spec_dlls} #{console_silent_flag} /nologo /nodots /process=Single /domain=Single "

  system mspec_runner_argument
  puts "\n===============================================================================================\n\n"
  system nunit_runner_argument
  puts "\n===============================================================================================\n"

end

task :turn_on_verbose_specs_output do
  output_silent = false
end

task :init_specs_run do
  output_html_report = ENV["html"] == "true"
end

task :init_teamcity_specs_run do
  output_silent = true
  output_teamcity_report = true
end

desc "Build and run specs"
task :default => [:build, :init_specs_run, :init_html_reportpath, :run_specs]

desc "Build and verbosely run specs" 
task :showall => [:turn_on_verbose_specs_output, :default]

desc "Build and show show teamcity report of specs"
task :teamcity => [:clean, :build, :init_teamcity_specs_run, :run_specs]
