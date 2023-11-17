namespace CompassWrapper
{
    using Kompas6API5;

    /// <summary>
    /// Обёртка для компаса.
    /// </summary>
    public class CompassWrapper
    {
        /// <summary>
        /// Имя идентификатора у приложения компас.
        /// </summary>
        private const string CompassProgramID = "KOMPAS.Application.5";

        /// <summary>
        /// Конструктор.
        /// </summary>
        public CompassWrapper()
        {
            KompasObject? compass = null;
            try
            {
                compass = MyMarshal.GetActiveObject(CompassProgramID) as KompasObject;
            }
            catch
            {
                if (compass == null)
                {
                    var compassType = Type.GetTypeFromProgID(CompassProgramID);
                    if (compassType != null)
                    {
                        compass = Activator.CreateInstance(compassType) as KompasObject;
                    }

                    if (compass == null)
                    {
                        // IApplication compassApp = compass.ksGetApplication7() as IApplication;
                    }

                    compass.Visible = true;
                }
            }

            compass.ActivateControllerAPI();
            Compass = compass;
        }

        /// <summary>
        /// Главный объект компаса.
        /// </summary>
        public KompasObject Compass { get; set; }
    }
}