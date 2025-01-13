using CompanionApp.Models;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanionApp.Events
{
    public class MenuSelectionChangedEvent : PubSubEvent<Section> { }
    public class LoadModuleEvent : PubSubEvent<Module> { }

    public class ShowSlidingViewEvent : PubSubEvent<bool> { }

}
