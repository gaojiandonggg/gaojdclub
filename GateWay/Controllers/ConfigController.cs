using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Ocelot.Configuration.Repository;
using Ocelot.Configuration.Setter;

namespace GateWay.Controllers
{
    public class ConfigController : Controller
    {

        public ConfigController(IFileConfigurationRepository repo,
            IFileConfigurationSetter setter)
        {

        }

    }
}
