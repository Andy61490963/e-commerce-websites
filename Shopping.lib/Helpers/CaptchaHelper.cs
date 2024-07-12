using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.Versioning;

namespace Shopping.lib.Helpers
{
    /// <summary>
    /// 生成驗證碼 Helper
    /// </summary>
    [SupportedOSPlatform( "windows" )]
    public class CaptchaHelper
    {
        public const string Key = "Captcha";

        /// <summary>
        /// 雜訊顏色清單
        /// </summary>
        private static readonly List<Color> NoiseColorList = new()
        {
            Color.LightYellow, Color.Cyan, Color.MistyRose, Color.PaleGreen, Color.PaleTurquoise,
            Color.Plum, Color.GreenYellow, Color.SpringGreen, Color.Thistle, Color.Lavender,
            Color.LightSlateGray, Color.DarkGray, Color.DarkSlateBlue, Color.SaddleBrown, Color.SlateGray,
            Color.SteelBlue, Color.Teal, Color.DarkSlateGray, Color.DarkOliveGreen, Color.MidnightBlue
        };

        /// <summary>
        /// 預設長度
        /// </summary>
        private const int DefaultLength = 4;

        /// <summary>
        /// 生成驗證碼
        /// </summary>
        /// <param name="length">長度 <see cref="DefaultLength"/></param>
        /// <param name="background">背景顏色</param>
        /// <param name="foreground">前景顏色</param>
        /// <param name="character">字元集合</param>
        /// <param name="noiseType">雜訊類型</param>
        public static CaptchaResult Generate( int length = DefaultLength,
                Color? background = null, 
                Color? foreground = null,
                Font? font = null,
                RandomHelper.Character character = RandomHelper.Character.Digit,
                NoiseType noiseType = NoiseType.None
            )
        {
            // 預設值
            background ??= Color.White;
            foreground ??= Color.Black;
            font ??= new Font( FontFamily.GenericSansSerif, 20 );
            length = length < DefaultLength ? DefaultLength : length;

            // 隨機生成驗證碼
            string captcha = RandomHelper.Next( length, character );

            // 建立驗證碼圖像
            var bitmap = new Bitmap( 1, 1 );
            var graphics = Graphics.FromImage( bitmap );
            var imgSize = graphics.MeasureString( captcha, font );
            graphics.Dispose();
            bitmap.Dispose();

            // 重新繪製圖片 (加上邊框留白)
            bitmap = new Bitmap( ((int)imgSize.Width) + 10, ((int)imgSize.Height) + 10 );
            graphics = Graphics.FromImage( bitmap );

            // 設定背景色
            graphics.Clear( background.Value );

            // 將驗證碼畫在圖像上
            graphics.DrawString( captcha, font, new SolidBrush( foreground.Value ), 5, 5 );

            // 加入雜訊
            AddNoise( graphics, bitmap.Size, noiseType );

            using( MemoryStream memoryStream = new MemoryStream() )
            {
                bitmap.Save( memoryStream, ImageFormat.Png );
                var base64String = Convert.ToBase64String( memoryStream.ToArray() );
                return new CaptchaResult( captcha, base64String );
            }
        }

        #region -- private methods --
        /// <summary>
        /// 新增雜訊
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="size"></param>
        /// <param name="noiseType"></param>
        private static void AddNoise( Graphics graphics, Size size, NoiseType noiseType )
        {
            var width = size.Width - 1;
            var height = size.Height - 1;

            // 取得雜訊數量的範圍 (100為一單位)
            var randRange = ( int ) Math.Ceiling( size.Width / 100.0 );
            // 基本雜訊數量倍率 = 3, 每 200 放大一單位
            var zoomRate = 3 + ( int ) Math.Floor( randRange / 2.0 );

            var pen = new Pen( Color.Gray, 1 );
            if( noiseType.HasFlag( NoiseType.Point ) )
            {
                int min = ( randRange + 1 ) * zoomRate * 10;
                int max = ( randRange + 2 ) * zoomRate * 10;
                int pointCount = RandomHelper.Next( min, max );
                for( var i = 0; i < pointCount; i++ )
                {
                    // 隨機取一個雜點的顏色
                    pen.Color = NoiseColorList[ RandomHelper.Next( 0, NoiseColorList.Count - 1 ) ];
                    // 隨機取雜點的位置
                    var x = RandomHelper.Next( 0, width - 1 );
                    var y = RandomHelper.Next( 0, height - 1 );
                    graphics.DrawRectangle( pen, x, y, 1, 1 );
                }
            }
            if( noiseType.HasFlag( NoiseType.Line ) )
            {
                var min = ( randRange + 1 ) * zoomRate;
                var max = ( randRange + 2 ) * zoomRate;
                var lineCount = RandomHelper.Next( min, max );
                for( var i = 0; i < lineCount; i++ )
                {
                    // 隨機取一條線的顏色
                    var c = RandomHelper.Next( 0, NoiseColorList.Count - 1 );
                    pen.Color = NoiseColorList[ c ];

                    // 隨機取線兩端的位置
                    var x1 = RandomHelper.Next( 0, width - 1 );
                    var y1 = RandomHelper.Next( 0, height - 1 );

                    var x2 = RandomHelper.Next( 0, width - 1 );
                    var y2 = RandomHelper.Next( 0, height - 1 );

                    graphics.DrawLine( pen, x1, y1, x2, y2 );
                }
            }
        }
        #endregion

        #region models
        /// <summary>
        /// 驗證碼結果
        /// </summary>
        public class CaptchaResult
        {
            public CaptchaResult( string captcha, string imageBase64String )
            {
                this.ImageBase64String = $"data:image/png;base64,{imageBase64String}";
                this.Captcha = captcha;
            }

            /// <summary>
            /// 圖片 Base 64 String
            /// </summary>
            public string ImageBase64String { get; }

            /// <summary>
            /// 驗證碼
            /// </summary>
            public string Captcha { get; }
        }
        #endregion

        #region enums
        /// <summary>
        /// 圖形雜訊類型
        /// </summary>
        [Flags]
        public enum NoiseType
        {
            /// <summary>
            /// 不需要雜訊
            /// </summary>
            None = 0,
            /// <summary>
            /// 點
            /// </summary>
            Point = 1,
            /// <summary>
            /// 線
            /// </summary>
            Line = 2,
        }
        #endregion
    }
}
