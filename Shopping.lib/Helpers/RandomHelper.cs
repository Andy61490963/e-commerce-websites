using Shopping.lib.Enums;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;

namespace Shopping.lib.Helpers
{
    /// <summary>
    /// 隨機 Helper
    /// </summary>
    public static class RandomHelper
    {
        private static readonly RandomNumberGenerator rng = RandomNumberGenerator.Create();

        /// <summary>
        /// 生成隨機數字
        /// </summary>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <returns></returns>
        public static int Next( int minValue, int maxValue )
        {
            byte[] bytes = new byte[ 4 ];
            rng.GetBytes( bytes );
            int randomNumber = BitConverter.ToInt32( bytes, 0 );
            return Math.Abs( randomNumber % ( maxValue - minValue ) ) + minValue;
        }

        /// <summary>
        /// 生成指定長度的隨機字串
        /// </summary>
        /// <param name="length"></param>
        /// <param name="character"></param>
        /// <returns></returns>
        public static string Next( int length, Character character )
        {
            var sb = new StringBuilder();
            var source = "";

            if( character.HasFlag( Character.Digit ) )
            {
                source += Character.Digit.GetDescription();
            }
            else if( character.HasFlag( Character.DigitNoDifficult ) )
            {
                source += Character.DigitNoDifficult.GetDescription();
            }
            if( character.HasFlag( Character.Lowercase ) )
            {
                source += Character.Lowercase.GetDescription();
            }
            else if( character.HasFlag( Character.LowercaseNoDifficult ) )
            {
                source += Character.LowercaseNoDifficult.GetDescription();
            }
            if( character.HasFlag( Character.Uppercase ) )
            {
                source += Character.Uppercase.GetDescription();
            }
            else if( character.HasFlag( Character.UppercaseNoDifficult ) )
            {
                source += Character.UppercaseNoDifficult.GetDescription();
            }
            if( character.HasFlag( Character.Special ) )
            {
                source += Character.Special.GetDescription();
            }

            for( int i = 0; i < length; i++ )
            {
                var index = RandomHelper.Next( 0, source.Length );
                sb.Append( source[ index ] );
            }
            return sb.ToString();
        }

        /// <summary>
        /// 字元類型
        /// </summary>
        [Flags]
        public enum Character
        {
            /// <summary>
            /// 數字
            /// </summary>
            [Display( Description = "0123456789" )]
            Digit = 1,
            /// <summary>
            /// 小寫英文
            /// </summary>
            [Display( Description = "abcdefghijklmnopqrstuvwxyz" )]
            Lowercase = 2,
            /// <summary>
            /// 大寫英文
            /// </summary>
            [Display( Description = "ABCDEFGHIJKLMNOPQRSTUVWXYZ" )]
            Uppercase = 4,
            /// <summary>
            /// 特殊符號
            /// </summary>
            [Display( Description = "!@#$%" )]
            Special = 8,
            /// <summary>
            /// 數字 (沒有難字)
            /// </summary>
            [Display( Description = "23456789" )]
            DigitNoDifficult = 16,
            /// <summary>
            /// 小寫英文 (沒有難字)
            /// </summary>
            [Display( Description = "abcdefghijkmnpqrstuvwxyz" )]
            LowercaseNoDifficult = 32,
            /// <summary>
            /// 大寫英文 (沒有難字)
            /// </summary>
            [Display( Description = "ABCDEFGHIJKMNPQRSTUVWXYZ" )]
            UppercaseNoDifficult = 64,
            /// <summary>
            /// 全部: 數字, 小寫英文, 大寫英文, 特殊符號
            /// </summary>
            All = Digit | Lowercase | Uppercase | Special,
            /// <summary>
            /// 全部: 數字, 小寫英文, 大寫英文, 特殊符號 (沒有難字)
            /// </summary>
            AllNoDifficult = DigitNoDifficult | LowercaseNoDifficult | UppercaseNoDifficult | Special,
        }
    }
}
