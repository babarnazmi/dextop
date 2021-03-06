﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Codaxy.Dextop.Modules;

namespace Codaxy.Dextop.Showcase
{
    public partial class ShowcaseApplication : DextopApplication
    {
        protected override void RegisterModules()
        {
            //Uncomment to use local ext resources
            //RegisterModule("client/lib/ext", new DextopExtJSModule
            //{
            //    CssThemeSuffix = "-gray"
            //});

            //RegisterModule("client/lib/ext/examples", new ExtJSDataViewModule());

            RegisterModule("http://dextop.codaxy.com/ext/extjs-4.1.1-rc1", new DextopExtJSModule
            {
                CssThemeSuffix = "-gray",
                UsingExternalResources = true
            });
            
            RegisterModule("http://dextop.codaxy.com/ext/extjs-4.1.1-rc1/examples", new ExtJSDataViewModule
            {
                UsingExternalResources = true
            });
            
            RegisterModule("client/lib/dextop", new DextopCoreModule());
			
			var soundModule = new DextopSoundModule();
			soundModule.AddSound("error", "client/sound/notify.mp3");
			RegisterModule("client/lib/sound", soundModule);

            RegisterModule("", new ShowcaseApplicationModule());
        }

        protected override void OnModulesInitialized()
        {
            base.OnModulesInitialized();
            if (Optimize)
                OptimizeModules("client/js/cache", !PreprocessorMode);

            if (PreprocessingEnabled && !PreprocessorMode)
                InitializeDemos();
        }        

        public bool Optimize { get; set; }      
    }
}