
using System.Collections.Generic;

public interface IActivatable {

	bool BecomeActivated(IActivator activator, Dictionary<object, object> argumentDictionary = null);

}
