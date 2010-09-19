
DOTNETVERSION = "4.0.30319"
DOTNETVERSIONMAJOR = "4.0"

MSBUILD = File.join(ENV["windir"] || ENV["WINDIR"], "Microsoft.Net", "Framework","v#{DOTNETVERSION}", "msbuild.exe")

CONFIG = ENV["config"] || ENV["CONFIG"] || "Debug"
MSBUILD_VERBOSITY_LEVEL = ENV["verbosity"] || "quiet" 
PROJECT_PATH = ENV["ProjectPath"].gsub('\\', '/')

CURRENT_PATH = File.dirname(__FILE__)
TOOLS_PATH = File.join("c:/dev/", "tools")
TEST_SUFFIX = "Tests"

output_silent = true

task :build do
  project = Dir.glob(File.join(PROJECT_PATH, "*.csproj")).first
  
  msbuild_succeeded = system "\"#{MSBUILD}\" \"#{project}\" /p:Configuration=#{CONFIG} /t:Build /tv:#{DOTNETVERSIONMAJOR} /verbosity:#{MSBUILD_VERBOSITY_LEVEL}"

  raise "Failed to build \"#{project}\"" unless msbuild_succeeded
end

task :run_specs do
  spec_dlls = Dir.glob(File.join(PROJECT_PATH, "/bin/#{CONFIG}/*.Tests.dll"))

	mspec_runner_exe = File.join(TOOLS_PATH, "mspec", "mspec.exe")
  console_silent_flag = output_silent ? '--silent' : ''
	mspec_runner_argument = "\"#{mspec_runner_exe}\" #{spec_dlls} #{console_silent_flag} "

  nunit_runner_exe = File.join(TOOLS_PATH,"NUnit/bin/net-2.0", "nunit-console.exe")
  console_silent_flag = output_silent ? '' : '' # '/labels'
  nunit_runner_argument = "\"#{nunit_runner_exe}\" #{spec_dlls} #{console_silent_flag} /nologo /nodots "

  system mspec_runner_argument
  puts "\n===============================================================================================\n\n"
  system nunit_runner_argument
  puts "\n===============================================================================================\n"

end

task :turn_on_verbose_specs_output do
  output_silent = false
end

task :default => [:build, :run_specs] do  end 
task :showall => [:turn_on_verbose_specs_output, :default] do end

