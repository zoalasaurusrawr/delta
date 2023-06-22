using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delta.Core.Models;
public class RegistryModel
{
    public RegistryKeyModel CurrentUser { get; set; } = new RegistryKeyModel();
    public RegistryKeyModel Users { get; set; } = new RegistryKeyModel();
    public RegistryKeyModel LocalMachine { get; set; } = new RegistryKeyModel();
    public RegistryKeyModel Configuration { get; set; } = new RegistryKeyModel();
}
