using Kompas6API5;
using Kompas6Constants3D;
using Model;

namespace CompassWrapper;

/// <summary>
/// Построитель киянки.
/// </summary>
public static class MalletBuilder
{
    /// <summary>
    /// Построить киянку по заданным параметрам.
    /// </summary>
    /// <param name="compassWrapper">Обёртка компаса.</param>
    /// <param name="malletParameters">Параметры киянки.</param>
    public static void Build(CompassWrapper compassWrapper, MalletParameters malletParameters)
    {
        var compass = compassWrapper.Compass;
        var document3d = CreateDocument3d(compass);
        var part = document3d?.GetPart((int)Part_Type.pTop_Part) as ksPart;
        var plane = part?.GetDefaultEntity((int)Obj3dType.o3d_planeXOY) as ksEntity;

        // Создаём модель бойка.
        var headSketch = CreateSketch(part, plane);
        var headDocument2d = headSketch?.BeginEdit() as ksDocument2D;
        DrawRectangle(headDocument2d, 0, 0, malletParameters.HeadLength, malletParameters.HeadWidth);
        headSketch?.EndEdit();
        MakeExtrusion(part, headSketch, malletParameters.HeadHeight);

        // Создаём модель рукоятки.
        var handleSketch = CreateSketch(part, plane);
        var handleDocumentd2 = handleSketch?.BeginEdit() as ksDocument2D;
        DrawCircle(handleDocumentd2, malletParameters.HeadLength / 2, malletParameters.HeadWidth / 2, malletParameters.HandleDiameter / 2);
        handleSketch?.EndEdit();
        MakeExtrusion(part, handleSketch, malletParameters.HandleHeight + malletParameters.HeadHeight);
    }

    /// <summary>
    /// Построить прямоугольника на заданной двумерной плоскости.
    /// </summary>
    /// <param name="document2d">Место для размещения прямоугольника.</param>
    /// <param name="x">Начальная координата прямоугольника по x.</param>
    /// <param name="y">Начальная координата прямоугольника по y.</param>
    /// <param name="width">Ширина прямоугольника.</param>
    /// <param name="height">Высота прямогольника.</param>
    private static void DrawRectangle(ksDocument2D document2d, double x, double y, double width, double height)
    {
        double x1 = x;
        double x2 = x + width;
        double y1 = y;
        double y2 = y + height;

        document2d.ksLineSeg(x1, y1, x2, y1, 1);
        document2d.ksLineSeg(x1, y1, x1, y2, 1);
        document2d.ksLineSeg(x2, y1, x2, y2, 1);
        document2d?.ksLineSeg(x1, y2, x2, y2, 1);
    }

    /// <summary>
    /// Построить окружность на заданной двумерной плоскости.
    /// </summary>
    /// <param name="document2d">Место для размещения окружности.</param>
    /// <param name="x">Координата центра окружности по x.</param>
    /// <param name="y">Координата центра окружности по y.</param>
    /// <param name="radius">Радиус окружности.</param>
    private static void DrawCircle(ksDocument2D document2d, double x, double y, double radius)
    {
        document2d.ksCircle(x, y, radius, 1);
    }

    /// <summary>
    /// Создаёт выдавливаение на заданном наброске.
    /// </summary>
    /// <param name="part">Деталь.</param>
    /// <param name="sketch">Набросок.</param>
    /// <param name="depth">Глубина выдавливания.</param>
    private static void MakeExtrusion(ksPart part, ksSketchDefinition sketch, double depth)
    {
        var extrusion = part.NewEntity((short)Obj3dType.o3d_bossExtrusion) as ksEntity;
        var extrusionDefinition = extrusion?.GetDefinition() as ksBossExtrusionDefinition;
        extrusionDefinition?.SetSideParam(true, (short)End_Type.etBlind, depth);
        extrusionDefinition?.SetSketch(sketch);
        extrusion?.Create();
    }

    /// <summary>
    /// Создаёт документ для трёхмерной модели.
    /// </summary>
    /// <param name="compass">Главный объект компаса.</param>
    /// <returns>Документ для трёхмерной модели.</returns>
    private static ksDocument3D? CreateDocument3d(KompasObject compass)
    {
        var document3d = compass.Document3D() as ksDocument3D;
        document3d?.Create(false, true);
        return document3d;
    }

    /// <summary>
    /// Создаёт набросок на заданной плоскости.
    /// </summary>
    /// <param name="part">Деталь.</param>
    /// <param name="plane">Плоскость.</param>
    /// <returns>Набросок.</returns>
    private static ksSketchDefinition? CreateSketch(ksPart part, ksEntity plane)
    {
        var sketch = part.NewEntity((int)Obj3dType.o3d_sketch) as ksEntity;
        var sketchDefinition = sketch?.GetDefinition() as ksSketchDefinition;
        sketchDefinition?.SetPlane(plane);
        sketch?.Create();
        return sketchDefinition;
    }
}
