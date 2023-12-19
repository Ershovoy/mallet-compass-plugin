namespace CompassWrapper
{
    using Kompas6API5;
    using Kompas6Constants3D;
    using Model;

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

            if (document3d == null)
            {
                return;
            }

            // Создаём модель бойка.
            BuildHead(
                document3d,
                malletParameters.HeadType,
                malletParameters.HeadDiameter,
                malletParameters.HeadWidth,
                malletParameters.HeadHeight,
                malletParameters.HeadLength);

            // Создаём модель рукоятки.
            BuildHandle(
                document3d,
                malletParameters.HandleHeight,
                malletParameters.HandleDiameter,
                malletParameters.HeadHeight);
        }

        /// <summary>
        /// Осуществляет построение бойка киянки.
        /// </summary>
        /// <param name="document3d">Документ для построения.</param>
        /// <param name="headType">Форма бойка.</param>
        /// <param name="headDiameter">Диаметр бойка.</param>
        /// <param name="headWidth">Ширина бойка.</param>
        /// <param name="headHeight">Высота бойка.</param>
        /// <param name="headLength">Длина бойка.</param>
        private static void BuildHead(
            ksDocument3D document3d,
            HeadType headType,
            double headDiameter,
            double headWidth,
            double headHeight,
            double headLength)
        {
            var headPart = document3d.GetPart((int)Part_Type.pTop_Part) as ksPart;
            var headPlane = headPart?.GetDefaultEntity((int)Obj3dType.o3d_planeYOZ) as ksEntity;

            if (headPart == null || headPlane == null)
            {
                return;
            }

            var headSketch = CreateSketch(headPart, headPlane);
            var headDocument2d = headSketch?.BeginEdit() as ksDocument2D;

            if (headSketch == null || headDocument2d == null)
            {
                return;
            }

            if (headType == HeadType.Rectangle)
            {
                DrawRectangle(
                    headDocument2d,
                    0,
                    0,
                    headWidth,
                    headHeight);
            }
            else
            {
                DrawCircle(headDocument2d, 0, 0, headDiameter / 2);
            }

            headSketch.EndEdit();

            MakeExtrusion(headPart, headSketch, headLength / 2);
            MakeExtrusion(headPart, headSketch, headLength / 2, false);

            if (headType == HeadType.Cylinder)
            {
                CreateChamfer(headPart, 10, headLength / 2, headWidth / 2, 0);
                CreateChamfer(headPart, 10, -headLength / 2, headWidth / 2, 0);
            }
        }

        /// <summary>
        /// Осуществляет построение рукоятки киянки.
        /// </summary>
        /// <param name="document3d">Документ для построения.</param>
        /// <param name="handleHeight">Высота рукоятки.</param>
        /// <param name="handleDiameter">Диаметр рукоятки.</param>
        /// <param name="headHeight">Высота бойка.</param>
        private static void BuildHandle(
            ksDocument3D document3d,
            double handleHeight,
            double handleDiameter,
            double headHeight)
        {
            var handlePart = document3d.GetPart((int)Part_Type.pTop_Part) as ksPart;
            var handlePlane = handlePart?.GetDefaultEntity((int)Obj3dType.o3d_planeXOY) as ksEntity;

            if (handlePart == null || handlePlane == null)
            {
                return;
            }

            var handleSketch = CreateSketch(handlePart, handlePlane);
            var handleDocument2d = handleSketch?.BeginEdit() as ksDocument2D;

            if (handleSketch == null || handleDocument2d == null)
            {
                return;
            }

            DrawCircle(handleDocument2d, 0, 0, handleDiameter / 2);
            handleSketch.EndEdit();
            MakeExtrusion(handlePart, handleSketch, handleHeight + headHeight);
        }

        /// <summary>
        /// Построить прямоугольника на заданной двумерной плоскости.
        /// </summary>
        /// <param name="document2d">Место для размещения прямоугольника.</param>
        /// <param name="x">Начальная координата прямоугольника по x.</param>
        /// <param name="y">Начальная координата прямоугольника по y.</param>
        /// <param name="width">Ширина прямоугольника.</param>
        /// <param name="height">Высота прямоугольника.</param>
        private static void DrawRectangle(
            ksDocument2D document2d,
            double x,
            double y,
            double width,
            double height)
        {
            var x1 = x - (height / 2);
            var x2 = x + (height / 2);
            var y1 = y - (width / 2);
            var y2 = y + (width / 2);

            document2d.ksLineSeg(x1, y1, x2, y1, 1);
            document2d.ksLineSeg(x1, y1, x1, y2, 1);
            document2d.ksLineSeg(x2, y1, x2, y2, 1);
            document2d.ksLineSeg(x1, y2, x2, y2, 1);
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
        /// Создаёт выдавливание на заданном наброске.
        /// </summary>
        /// <param name="part">Деталь.</param>
        /// <param name="sketch">Набросок.</param>
        /// <param name="depth">Глубина выдавливания.</param>
        /// <param name="side">Направление выдавливания.</param>
        private static void MakeExtrusion(
            ksPart part,
            ksSketchDefinition sketch,
            double depth,
            bool side = true)
        {
            var extrusion = part.NewEntity((short)Obj3dType.o3d_bossExtrusion) as ksEntity;
            var extrusionDefinition = extrusion?.GetDefinition() as ksBossExtrusionDefinition;
            if (extrusionDefinition != null)
            {
                extrusionDefinition.SetSideParam(side, (short)End_Type.etBlind, depth);
                extrusionDefinition.directionType =
                    side ? (short)Direction_Type.dtNormal : (short)Direction_Type.dtReverse;
                extrusionDefinition.SetSketch(sketch);
            }

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
            document3d?.Create();
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

        /// <summary>
        /// Метод для создания фаски на выбранном ребре.
        /// </summary>
        /// <param name="part">Деталь.</param>
        /// <param name="chamferRadius">Радиус фаски.</param>
        /// <param name="x">Координата ребра по X.</param>
        /// <param name="y">Координата ребра по Y.</param>
        /// <param name="z">Координата ребра по Z.</param>
        private static void CreateChamfer(
            ksPart part,
            double chamferRadius,
            double x,
            double y,
            double z)
        {
            var chamferEntity = part.NewEntity((short)Obj3dType.o3d_fillet) as ksEntity;
            var chamferDefinition = chamferEntity?.GetDefinition() as ksFilletDefinition;

            if (chamferDefinition == null)
            {
                return;
            }

            chamferDefinition.radius = chamferRadius;
            chamferDefinition.tangent = true;

            var entityArray = chamferDefinition.array() as ksEntityCollection;
            var entityCollection = part.EntityCollection((short)Obj3dType.o3d_edge) as ksEntityCollection;
            entityCollection?.SelectByPoint(x, y, z);

            var entityEdge = entityCollection?.Last();
            entityArray?.Add(entityEdge);

            chamferEntity?.Create();
        }
    }
}