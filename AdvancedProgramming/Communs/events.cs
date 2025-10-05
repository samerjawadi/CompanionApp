using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedProgramming.Events
{
    public class ScriptChangedEvent : PubSubEvent<string> { }
    public class ScriptLoadedEvent : PubSubEvent<string> { }

}
