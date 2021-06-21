# CADReader

This library can be used for reading CAD files (version 4). Currently it reads only `HEADER` and `GRAPHICS` sections. The `SEMANTIC` sections is left unprocessed. Only CAD files v4 are supported!

## Install

Install with Nuget: `Install-Package CADReader`.

## Usage

Аdd a reference to `CADReader` and then instantiate a new instance of `CADFile` class:

```csharp
using CAD;
using CAD.Entity;
using CAD.Geometry;
using CAD.Nomenclature;
...
// read a CAD file
CADFile file = new CADFile("path/to/cad/file");

// get CAD file metadata
CADFileInfo info = file.FileInfo;

// access all layers
List<CADLayer> layers = file.Layers;
// or specific layer
CADLayer cadaster = file[CADLayerType.CADASTER];

// access entities
CADPoint point = cadaster.Entities[0] as CADPoint;

// access entity's geometry
Point geometry = point.Geometry as Point;
```

## Entities

Following entities are described in CAD format and are supported by this library:

- `CADLayer` - defines a nested section in the `GRAPHICS` section, grouping 1 or more graphics
- `CADPoint` - defines point graphics - either points from a geodetical network or polylines.
- `CADLine` - defines line geometries
- `CADContour` - defines polygon geometries
- `CADSymbol` - defines point symbols
- `CADText` - defines texts

### CADLayer

Each `CADLayer` contains graphics and provides methods for searching using the spatial index, in which all graphics are inserted. Each layer has its separate spatial index created.

#### **Search by Properties**

You can provide a filter function and search for graphics, satisfying your filter:

```csharp
// search by entity type
List<ICADEntity> pointEntities = layer
    .Search(i => i is CADPoint);

// search by CAD ID
string cadID = "6849.210";
ICADEntity result = layer.Search(i =>
{
    if (i is CADContour contour && contour.CADId.Equals(cadID))
        return true;
    return false;
}).FirstOrDefault();
```

#### **Search in Extent**

You can search for all graphics within the specified extent:

```csharp
// create the search extent
Extent envelope = new Extent(631.362, 1553.444, 638.741, 1561.818);
// search for graphics
List<ICADEntity> results = layer.Search(envelope);
```

## Geometries

- `Extent` - used as search geometry when running spatial queries within layers
- `Point` - geometry representation of point graphics like `CADPoint`
- `Polyline` - geometry representation of line graphics like `CADLine`
- `Polygon` - geometry representation of polygon graphics like `CADContour`

## Nomenclatures

Contains additional classes and enumerations with code/value pairs.
