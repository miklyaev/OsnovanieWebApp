using AutoWrapper;

namespace OsnovanieWebApp.Model
{
    public class MapResponseObject
    {
        [AutoWrapperPropertyMap(Prop.Result)]
        public object Data { get; set; }
    }
}
