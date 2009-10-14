using System;
using System.Configuration;

namespace PokerTell.Infrastructure
{
	public class UserData
	{
	    static UserData instance;
	    public static Configuration Config {
	        get {
	            if(instance == null)
	                instance = new UserData();
	            
	            return instance.config;;
	        }
	    }
	    
	    private UserData()
	    {
	        string configFile = Files.dirAppData + Files.xmlUserConfig;
	       
	        // Map the new configuration file.
	        ExeConfigurationFileMap configFileMap =
	            new ExeConfigurationFileMap();
	        configFileMap.ExeConfigFilename = configFile;
	        
	        // Get the mapped configuration file
	        
	        this.config = ConfigurationManager.OpenMappedExeConfiguration(
	            configFileMap, ConfigurationUserLevel.None);
	    }
	    
	    Configuration config;
	    
	}
}
