using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumericalAnalysis
{
    partial class Algorithms
    {

        // Sử dụng Hàm SVD để khai triển ma trận thành tích 3 ma trận U*S*Vt
        /// <summary>
        /// Hàm khai triển ma trận m*n thành tích 3 ma trận U*S*Vt, với giả thiết bắt buộc là m>n
        /// </summary>
        /// <param name="matrix">Ma trận cần khai triển (m*n)</param>
        /// <param name="U">Ma trận kì dị phải (m*m)</param>
        /// <param name="S">Ma trận đường chéo S (m*n)</param>
        /// <param name="Vt">Ma trận kì dị trái (n*n)</param>
        public static void SVDDecompositionMain(double[,] matrix, out double[,] U, out double[,] S, out double[,] Vt)
        {
            // Cho ma trận A là ma trận cỡ m*n
            // Để khai triển A, ta xét 2 loại ma trận AtA (m*m) và AAt (n*n)

            U = S = Vt = null;

            /* FindRankA: đi tìm rank của ma trận A, trả về hạng của ma trận A  */

            /* NullityA : Đi tìm trị số nullity của A, = n - rankA, trả về số lượng tối đa các vecto v
             * Độc lập tuyến tính trong không gian Rn mà Av = 0 */

            /* Ta chứng minh được : AtA và AAt là 2 ma trận đối xứng, rank A = rank (AtA), giá trị riêng 
             * của 2 ma trận này đều là số thực dương, các giá trị riêng khác 0 của 2 ma trận này đều bằng 
             * nhau */

            /* Ta tìm S như sau: tìm trị riêng của ma trận AtA , sắp xếp theo thứ tự lớn đến bé từ trên
             * xuống căn bậc 2 trị riêng vừa tìm được là ta
             * có S.*/

            // Cần có một gói chương trình đi tìm trị riêng, vecto riêng... mới ra ma trận V

        }
    }
}
