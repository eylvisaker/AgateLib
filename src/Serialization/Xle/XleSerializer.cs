using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Reflection;

namespace ERY.AgateLib.Serialization.Xle
{
    public class XleSerializer 
    {
        Type objectType;

        public ITypeBinder Binder { get; set; }

        public XleSerializer(Type objectType)
        {
            if (objectType.GetInterface("IXleSerializable", true) == null)
                throw new ArgumentException("Object type is not IXleSerializable.");

            Binder = new TypeBinder();
            (Binder as TypeBinder).AddAssembly(Assembly.GetCallingAssembly());
            (Binder as TypeBinder).SearchAssemblies.Add(Assembly.GetExecutingAssembly());

            this.objectType = objectType;
        }

        public void Serialize(Stream outStream, IXleSerializable objectGraph)
        {
            if (objectType.IsAssignableFrom(objectGraph.GetType()) == false)
                throw new ArgumentException("Object is not of type " + objectType.GetType());

            XleSerializationInfo info = new XleSerializationInfo();

            info.Binder = Binder;
            info.BeginSerialize(objectGraph);

            info.XmlDoc.Save(outStream);

        }

        public object Deserialize(Stream file)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(file);
            XleSerializationInfo info = new XleSerializationInfo(doc);

            info.Binder = Binder;
            return info.BeginDeserialize();
        }

        public T Deserialize<T>(Stream file)
        {
            return (T)Deserialize(file);
        }
    }
}
