using Newtonsoft.Json;
using System.Drawing.Imaging;
using System.Text;
using System.Text.RegularExpressions;

namespace RaceBoard.Common
{
    public class Formatter
    {
        public static class JSON<T>
        {
            public static string Serialize(T value)
            {
                return JsonConvert.SerializeObject(value);
            }
            public static T Deserialize(string value)
            {
                return JsonConvert.DeserializeObject<T>(value);
            }
        }

        #region Imágenes

        /// <summary>
        /// Clase para conversiones de array de Bytes
        /// </summary>
        public static class ByteArray
        {
            /// <summary>
            /// Convierte una imagen en un array de bytes
            /// </summary>
            /// <param name="image"></param>
            /// <param name="format"></param>
            /// <returns></returns>
            public static byte[] ConvertToByteArray(System.Drawing.Image image, ImageFormat format)
            {
                MemoryStream ms = new MemoryStream();
                image.Save(ms, format);

                return ms.ToArray();
            }

            /// <summary>
            /// Convierte un array de bytes en una imagen
            /// </summary>
            /// <param name="array"></param>
            /// <returns></returns>
            public static System.Drawing.Image ConvertToImage(byte[] array)
            {
                MemoryStream ms = new MemoryStream(array);
                System.Drawing.Image returnImage = System.Drawing.Image.FromStream(ms);

                return returnImage;
            }
        }

        /// <summary>
        /// Clase para conversiones del formato Base64
        /// </summary>
        public static class Base64
        {
            /// <summary>
            /// Convierte un array de Bytes en su representación en Base64
            /// </summary>
            /// <param name="image"></param>
            /// <returns></returns>
            public static string ConvertirEnBase64(byte[] array)
            {
                string base64 = string.Empty;
                if (array != null)
                {
                    base64 = Convert.ToBase64String(array);
                }
                return base64;
            }

            /// <summary>
            /// Convierte una representación en Base64 a un array de bytes
            /// </summary>
            /// <param name="base64Encoding"></param>
            /// <returns></returns>
            public static byte[] ConvertirEnArrayDeBytes(string base64)
            {
                byte[] imagen = new byte[] { };
                if (base64 != null)
                {
                    imagen = Convert.FromBase64String(base64);
                }
                return imagen;
            }
        }
        #endregion

        #region Streams

        /// <summary>
        /// Clase para conversiones de Streams
        /// </summary>
        public static class IO_Stream
        {
            /// <summary>
            /// Convierte el contenido de un Stream a un array de bytes
            /// </summary>
            /// <param name="input"></param>
            /// <returns></returns>
            public static byte[] ConvertirEnArray(Stream stream)
            {
                byte[] buffer = new byte[stream.Length];
                using (MemoryStream ms = new MemoryStream())
                {
                    int read;
                    while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        ms.Write(buffer, 0, read);
                    }
                    return ms.ToArray();
                }
            }
        }
        #endregion

        #region Text

        /// <summary>
        /// Clase para manipulación de cadenas de texto
        /// </summary>
        public static class Text
        {
            /// <summary>
            /// Elimina los caracteres excedentes que se encuentren tras la longitud indicada
            /// </summary>
            /// <param name="texto"></param>
            /// <param name="longitud"></param>
            /// <returns></returns>
            public static string ReducirTexto(string texto, int longitud)
            {
                if (texto.Length > longitud)
                    texto = texto.Substring(0, longitud) + "...";
                return texto;
            }

            /// <summary>
            /// Convierte un texto a formato CamelCase (Ejemplo: CamelCase)
            /// </summary>
            /// <param name="text"></param>
            /// <returns></returns>
            public static string ToCamelCase(string text)
            {
                string[] words = text.Split(' ');
                StringBuilder sb = new StringBuilder();
                foreach (string s in words)
                {
                    string firstLetter = s.Substring(0, 1);
                    string rest = s.Substring(1, s.Length - 1);
                    sb.Append(firstLetter.ToUpper() + rest.ToLower());
                }
                return sb.ToString();
            }

            /// <summary>
            /// Convierte un texto a formato PascalCase (Ejemplo: pascalCase)
            /// </summary>
            /// <param name="text"></param>
            /// <returns></returns>
            public static string ToPascalCase(string text)
            {
                string[] words = text.Split(' ');
                StringBuilder sb = new StringBuilder();
                bool isFirstWord = true;
                foreach (string s in words)
                {
                    string firstLetter = s.Substring(0, 1);
                    string rest = s.Substring(1, s.Length - 1);
                    if (isFirstWord)
                        sb.Append(firstLetter.ToLower() + rest.ToLower());
                    else
                        sb.Append(firstLetter.ToUpper() + rest.ToLower());
                    isFirstWord = false;
                }
                return sb.ToString();
            }
        }

        #endregion

        #region Null

        /// <summary>
        /// Clase para conversiones de NULL
        /// </summary>
        public static class Null
        {
            /// <summary>
            /// Convierte un Null a String
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            public static string ToString(string value)
            {
                string _value = string.Empty;
                try
                {
                    if (value != null)
                        _value = value;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return _value;
            }

        }

        #endregion

        public static bool ToBoolean(object value)
        {
            var type = value.GetType();
            var iValue = 0;
            if (type == typeof(string))
            {

                switch (value.ToString().ToLower())
                {
                    case "1":
                    case "true":
                        iValue = 1;
                        break;
                    default:
                        iValue = 0;
                        break;
                }
            }
            return Convert.ToBoolean(iValue);
        }

        public static string ToInteger(object value)
        {
            return String.Format("{0:N0}", value);
        }
        public static string ToFixed(object value)
        {
            return String.Format("{0:N2}", value);
        }
        public static string ToCurrency(object value)
        {
            return String.Format("{0:C}", value);
        }
        public static string ToDateTime(DateTime value, string format)
        {
            var _format = "dd/MM/yyyy";
            if (!string.IsNullOrEmpty(format))
                _format = format;
            var _timeFormat = " HH:mm:ss";
            return String.Format("{0:" + _format + _timeFormat + "}", value);
        }
        public static string RemoveNonNumeric(string input)
        {
            //exclude any character that is not a comma ",", a period ".", or a number within range 0 to 9..
            var r = new Regex("[^.,0-9]", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
            return r.Replace(input, String.Empty);
        }
        public static string RemoveSpecialCharacters(string input)
        {
            //exclude any character that is not a blank space, or within ranges a to z, 0 to 9..
            var r = new Regex("(?:[^a-z0-9 ]|(?<=['\"])s)", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
            return r.Replace(input, String.Empty);
        }

    }
}
