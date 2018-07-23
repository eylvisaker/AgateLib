using AgateLib;
using AgateLib.Mathematics;
using AgateLib.Mathematics.Geometry;
using AgateLib.Parsers.Tmx;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace AgateLib.Parsers.Tmx
{
    public interface ITmxLoader
    {
        TmxData Load(string filename);
    }

    [Singleton]
    public class TmxLoader : ITmxLoader
    {
        private readonly IContentProvider contentProvider;

        public TmxLoader(IContentProvider contentProvider)
        {
            this.contentProvider = contentProvider;
        }

        public TmxData Load(string filename)
        {
            string localfile = filename;
            string subdirectory = "";
            NormalizePath(ref subdirectory, ref localfile);

            using (var stream = contentProvider.Open(filename))
            {
                TmxData result = new TmxData();

                XDocument doc = XDocument.Load(stream);
                XElement root = doc.Element("map");

                if (root == null)
                    throw new InvalidDataException("Map did not contain root element \"map\"");

                result.Subdirectory = subdirectory;
                result.Size = root.AttributesToSize("width", "height");
                result.TileSize = root.AttributesToSize("tilewidth", "tileheight");

                foreach (var element in root.Elements())
                {
                    switch (element.Name.LocalName)
                    {
                        case "properties":
                            ReadProperties(result.Properties, element);
                            break;

                        case "tileset":
                            result.TileSets.Add(ReadTileset(element, subdirectory));
                            break;

                        case "objectgroup":
                        case "layer":
                            result.Layers.Add(ReadLayer(element));
                            break;

                        default:
                            Log.Debug($"Skipping unknown map element {element.Name}.");
                            break;
                    }
                }

                return result;
            }
        }


        private TmxTileSetData ReadTileset(XElement element, string subdirectory)
        {
            var source = element.AttributeToString("source", null);

            var result = new TmxTileSetData();

            if (source != null)
            {
                var path = string.IsNullOrWhiteSpace(subdirectory) ? source : $"{subdirectory}/{source}";
                var thisSubDir = PathX.GetDirectoryName(path);

                using (var stream = contentProvider.Open(path))
                {
                    XDocument doc = XDocument.Load(stream);

                    result = ReadTileset(doc.Element("tileset"), thisSubDir);
                }
            }
            else
            {
                result.Name = element.AttributeToString("name");
                result.TileSize = element.AttributesToSize("tilewidth", "tileheight");
                result.TileCount = element.AttributeToInt("tilecount");
                result.Columns = element.AttributeToInt("columns");

                ReadProperties(result.Properties, element.Element("properties"));

                var image = element.Element("image");

                result.ImageSource = PathX.NormalizePath(
                    Path.Combine(subdirectory, image.AttributeToString("source")));
                result.ImageSize = image.AttributesToSize("width", "height");

                foreach (var eTile in element.Elements("tile"))
                {
                    var id = eTile.AttributeToInt("id");
                    var properties = eTile.Element("properties");

                    var tileData = new TmxTileData();
                    result.Tiles[id] = tileData;

                    ReadProperties(result.Tiles[id].Properties, properties);

                    var objects = eTile.Element("objectgroup");

                    foreach (var obj in objects?.Elements("object") ?? Enumerable.Empty<XElement>())
                    {
                        tileData.Collision.Add(ReadMapObjectData(obj));
                    }
                }
            }

            result.FirstTileId = element.AttributeToInt("firstgid", 1);

            return result;
        }

        private TmxLayerData ReadLayer(XElement layerElement)
        {
            var result = new TmxLayerData();

            result.Name = layerElement.AttributeToString("name");
            result.Size = layerElement.AttributesToSize("width", "height", Size.Empty);

            foreach (var element in layerElement.Elements())
            {
                switch (element.Name.LocalName)
                {
                    case "properties":
                        ReadProperties(result.Properties, element);
                        break;

                    case "data":
                        result.TileData = ReadData(element).ToArray();
                        break;

                    case "object":
                        result.Objects.Add(ReadMapObjectData(element));
                        break;
                }
            }

            return result;
        }

        private void NormalizePath(ref string subdirectory, ref string filename)
        {
            filename = filename.Replace('\\', '/');

            if (filename.Contains("/"))
            {
                var index = filename.LastIndexOf('/');
                var filenameSubdirectory = filename.Substring(0, index);

                filename = filename.Substring(index + 1);

                if (string.IsNullOrWhiteSpace(subdirectory))
                {
                    subdirectory = filenameSubdirectory;
                }
                else
                {
                    subdirectory += "/" + filenameSubdirectory;
                }
            }
        }

        private TmxObjectData ReadMapObjectData(XElement element)
        {
            var id = element.AttributeToInt("id");
            var name = element.AttributeToString("name", null);
            var type = element.AttributeToString("type", null);

            bool isRectangle = element.Attribute("width") != null;
            bool isPolygon = element.Element("polygon") != null;
            bool isPolyLine = element.Element("polyline") != null;

            var result = new TmxObjectData();

            result.Id = id;
            result.Name = name;
            result.Type = type;

            var pt = element.AttributesToVector2("x", "y");

            if (isRectangle)
            {
                result.DataType = MapObjectDataType.Rectangle;
                result.Polygon = new RectangleF(pt, element.AttributesToSizeF("width", "height"))
                    .ToPolygon();
            }
            else if (isPolygon)
            {
                result.DataType = MapObjectDataType.Polygon;
                result.Polygon = ParsePolygon(element.Element("polygon").Attribute("points").Value)
                    .Translate(pt);
            }
            else if (isPolyLine)
            {
                result.DataType = MapObjectDataType.Polyline;
                result.Polygon = ParsePolygon(element.Element("polyline").Attribute("points").Value)
                    .Translate(pt);
            }

            ReadProperties(result.Properties, element.Element("properties"));

            return result;
        }

        private IEnumerable<int> ReadData(XElement element)
        {
            var encoding = element.AttributeToString("encoding");
            var contents = element.Value;

            switch (encoding)
            {
                case "csv":
                    return contents.Split(',').Select(int.Parse);

                default:
                    throw new NotImplementedException($"Decoding not implmented for {encoding}.");
            }
        }

        private void ReadProperties(PropertyBag properties, XElement propertiesElement)
        {
            foreach (var prop in propertiesElement?.Elements("property") ?? Enumerable.Empty<XElement>())
            {
                properties[prop.AttributeToString("name")] = prop.AttributeToString("value");
            }
        }

        private Polygon ParsePolygon(string value)
        {
            var pointStrings = value.Split(' ');

            Polygon result = new Polygon();

            foreach (var pt in pointStrings)
            {
                result.Add(Vector2X.Parse(pt));
            }

            return result;
        }
    }

    public static class TmxExtensions
    {
        public static TmxData Load(this ITmxLoader loader, IContentProvider contentProvider, string filePath)
        {
            return loader.Load(contentProvider.ReadAllText(filePath));
        }
    }
}
