namespace CompassWrapper
{
    using System.Runtime.InteropServices;
    using Kompas6API5;

    /// <summary>
    /// Обёртка для компаса.
    /// </summary>
    public class CompassWrapper
    {
        /// <summary>
        /// Имя идентификатора у приложения компас.
        /// </summary>
        private const string CompassProgramId = "KOMPAS.Application.5";

        /// <summary>
        /// Главный объект компаса.
        /// </summary>
        private KompasObject? _compass;

        /// <summary>
        /// Конструктор.
        /// </summary>
        public CompassWrapper() { }

        /// <summary>
        /// Главный объект компаса.
        /// </summary>
        public KompasObject Compass
        {
            get => ConnectToCompass();
            set => _compass = value;
        }

        /// <summary>
        /// Метод осуществляет подключение к api компаса.
        /// </summary>
        /// <returns>Главный объект компаса.</returns>
        /// <exception cref="Exception">Не удалось подключиться к КОМПАС-3D.</exception>
        private KompasObject ConnectToCompass()
        {
            KompasObject? compass = null;
            try
            {
                compass = MyMarshal.GetActiveObject(CompassProgramId) as KompasObject;
            }
            catch
            {
                if (compass == null)
                {
                    var compassType = Type.GetTypeFromProgID(CompassProgramId);
                    if (compassType != null)
                    {
                        compass = Activator.CreateInstance(compassType) as KompasObject;
                    }

                    if (compass == null)
                    {
                        throw new Exception("Не удалось подключиться к КОМПАС-3D");
                    }

                    compass.Visible = true;
                }
            }

            return compass;
        }
    }
}