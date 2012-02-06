using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Runtime.InteropServices;
using System.ComponentModel;

namespace ImageLib_ss {
    [TypeConverter(typeof(PropertyDisplayConverter))]
    public class Exif {

        public Dictionary<int, string> OrientationValueDict = new Dictionary<int, string>();
        public Dictionary<int, string> ExposureProgramValueDict = new Dictionary<int, string>();
        public Dictionary<int, string> MeteringModeValueDict = new Dictionary<int, string>();
        public Dictionary<int, string> LightSourceValueDict = new Dictionary<int, string>();
        public Dictionary<int, string> FlashValueDict = new Dictionary<int, string>();
        public Dictionary<int, string> ColorSpaceValueDict = new Dictionary<int, string>();
        public Dictionary<int, string> FocalPlaneResolutionUnitValueDict = new Dictionary<int, string>();
        public Dictionary<int, string> SensingMethodValueDict = new Dictionary<int, string>();

        private int[] _dataLength = { -1, 1, 1, 2, 4, 8, 1, 1, 2, 4, 8, 4, 8 };

        private Bitmap _Image;
        private byte[] _imgByte;

        // header プロパティ
        private bool _IsJpeg = false;
        private bool _ContainApp0 = false;
        private bool _ContainApp1 = false;
        private bool _IsExif = false;

        // Tiff Tag
        private string _ImageDescription = null;
        private string _Make = null;
        private string _Model = null;
        private string _Orientation = null;
        private string _XResolution = null;
        private string _YResolution = null;
        private ushort? _ResolutionUnit = null;
        private string _Software = null;
        private DateTime _Date ;
        private string _WhitePoint = null;
        private string _PrimaryChromaticities = null;
        private string _YCbCrCoefficients = null;
        private ushort? _YCbCrPositioning = null;
        private string _ReferenceBlackWhite = null;
        private string _Copyright = null;
        private uint? _ExifIFDPinter = null;

        // Exif Tag 
        private string _ExposureTime = null;
        private string _FNumber = null;
        private string _ExposureProgram = null;
        private ushort? _ISOSpeedRatings = null;
        private string _ExifVersion = null;
        private DateTime _DateTimeOriginal;
        private DateTime _DateTimeDigitized;
        private string _ComponentsConfiguration = null;
        private string _CompressedBitsPerPixel = null;
        private double? _ShutterSpeedValue = null;
        private double? _ApertureValue = null;
        private double? _BrightnessValue = null;
        private double? _ExposureBiasValue = null;
        private double? _MaxApertureValue = null;
        private double? _SubjectDistance = null;
        private string _MeteringMode = null;
        private string _LightSource = null;
        private ushort? _Flash = null;
        private double? _FocasLength = null;
        private string _MakerNote = null;
        private string _UserComment = null;
        private string _SubsecTime = null;
        private string _SubsecTimeOriginal = null;
        private string _SubsecTimeDigitized = null;
        private string _FlashPixVersion = null;
        private string _ColorSpace = null;
        private uint? _ExifImageWidth = null;
        private uint? _ExifImageHeight = null;
        private string _RelatedSoundFile = null;
        private uint? _InteroperabilityIFDPointer = null;
        private string _FocalPlaneXResolution = null;
        private string _FocalPlaneYResolution = null;
        private string _FocalPlaneResolutionUnit = null;
        private string _ExposureIndex = null;
        private string _SensingMethod = null;
        private string _FileSource = null;
        private string _SceneType = null;
        private string _CFAPattern = null;

        #region Property

        [Category("Image"),Description("イメージのプロパティを表します。")]
        public Bitmap Image { get { return _Image; } }


        // header プロパティ
        [Category("内部項目"), Description("JPEGの可否を表します。"),PropertyDisplayName("Jpeg?")]
        public bool IsJpeg { get { return _IsJpeg; } }
        [Category("内部項目"), Description("APP1マーカの有り無しを表します。"), PropertyDisplayName("App1?")]
        public bool ContainApp1 { get { return _ContainApp1; } }
        [Category("内部項目"), Description("Exifの可否を表します。"), PropertyDisplayName("Exif?")]
        public bool IsExif { get { return _IsExif; } }

        // Tiff Tag
        [Category("画像情報"), Description("Asciiコードにて画像の説明を表します。"), PropertyDisplayName("画像説明")]
        public string ImageDescription { get { return _ImageDescription; } }
        [Category("画像情報"), Description("Asciiコードにてメーカ名を表します"), PropertyDisplayName("メーカ名")]
        public string Make { get { return _Make; } }
        [Category("画像情報"), Description("Asciiコードにてカメラの機種名を表します"), PropertyDisplayName("機種名")]
        public string Model { get { return _Model; } }
        [Category("画像情報"), Description("画像の開始位置を表します。"), PropertyDisplayName("起点位置")]
        public string Orientation { get { return _Orientation; } }
        [Category("画像情報"), Description("横方向のの表示・印刷分解能を表します。"), PropertyDisplayName("横方向分解能")]
        public string XResolution { get { return _XResolution; } }
        [Category("画像情報"), Description("縦方向のの表示・印刷分解能を表します。"), PropertyDisplayName("縦方向分解能")]
        public string YResolution { get { return _YResolution; } }
        [Category("画像情報"), Description("分解能の単位を表します。"), PropertyDisplayName("分解能単位")]
        public ushort? ResolutionUnit { get { return _ResolutionUnit; } }
        [Category("画像情報"), Description("デジタルカメラの内臓ソフトウェアのバージョンを表します。"), PropertyDisplayName("内臓ソフトウェアバージョン")]
        public string Software { get { return _Software; } }
        [Category("画像情報"), Description("画像の最終変更日時を表します。"), PropertyDisplayName("最終変更日時")]
        public DateTime Date { get { return _Date; } }
        [Category("画像情報"), Description("白色の色度を定義します。"), PropertyDisplayName("白色定義")]
        public string WhitePoint { get { return _WhitePoint; } }
        [Category("画像情報"), Description("原色の色度を定義します。"), PropertyDisplayName("原色定義")]
        public string PrimaryChromaticities { get { return _PrimaryChromaticities; } }
        [Category("画像情報"), Description("YCbCr定数を定義します。"), PropertyDisplayName("YCbCr定数")]
        public string YCbCrCoefficients { get { return _YCbCrCoefficients; } }
        [Category("画像情報"), Description("色情報のサンプリング間引きを定義します。"), PropertyDisplayName("間引き方法")]
        public ushort? YCbCrPositioning { get { return _YCbCrPositioning; } }
        [Category("画像情報"), Description("黒点・白点の値を表します。"), PropertyDisplayName("黒白点値")]
        public string ReferenceBlackWhite { get { return _ReferenceBlackWhite; } }
        [Category("画像情報"), Description("著作権情報を表します。"), PropertyDisplayName("Copyright")]
        public string Copyright { get { return _Copyright; } }
        [Category("内部項目"), Description("Exif Sub IFDへのオフセットを表します。"), PropertyDisplayName("Exif Pointer")]
        public uint? ExifIFDPinter { get { return _ExifIFDPinter; } }

        // Exif Tag 
        [Category("撮影項目"), Description("露光時間を表します。"), PropertyDisplayName("露光時間")]
        public string ExposureTime { get { return _ExposureTime; } }
        [Category("撮影項目"), Description("レンズF値を表します。"), PropertyDisplayName("レンズF値")]
        public string FNumber { get { return _FNumber; } }
        [Category("撮影項目"), Description("露光制御モードを表します。"), PropertyDisplayName("露光制御モード")]
        public string ExposureProgram { get { return _ExposureProgram; } }
        [Category("撮影項目"), Description("CCD感度の銀塩フィルム換算値を表します。"), PropertyDisplayName("ISO感度")]
        public ushort? ISOSpeedRatings { get { return _ISOSpeedRatings; } }
        [Category("撮影項目"), Description("Exifバージョンを表します。"), PropertyDisplayName("Exifバージョン")]
        public string ExifVersion { get { return _ExifVersion; } }
        [Category("撮影項目"), Description("撮影時間を表します。"), PropertyDisplayName("撮影時間")]
        public DateTime DateTimeOriginal { get { return _DateTimeOriginal; } }
        [Category("撮影項目"), Description("デジタル化時間を表します。"), PropertyDisplayName("デジタル化時間")]
        public DateTime DateTimeDigitized { get { return _DateTimeDigitized; } }
        [Category("画像情報"), Description("格納されているデータの並びを表します。"), PropertyDisplayName("ComponentsConfiguration")]
        public string ComponentsConfiguration { get { return _ComponentsConfiguration; } }
        [Category("画像情報"), Description("JPEG圧縮率を表します。"), PropertyDisplayName("圧縮率")]
        public string CompressedBitsPerPixel { get { return _CompressedBitsPerPixel; } }
        [Category("撮影項目"), Description("シャッタースピードをAPEX値(TV)で表します。"), PropertyDisplayName("シャッタスピード(APEX)")]
        public double? ShutterSpeedValue { get { return _ShutterSpeedValue; } }
        [Category("撮影項目"), Description("レンズ絞りをAPEX値(AV)で表します。"), PropertyDisplayName("レンズ絞り(APEX)")]
        public double? ApertureValue { get { return _ApertureValue; } }
        [Category("撮影項目"), Description("明るさをAPEX値で表します。"), PropertyDisplayName("明るさ")]
        public double? BrightnessValue { get { return _BrightnessValue; } }
        [Category("撮影項目"), Description("露光補正量をAPEX値(EV)を表します。"), PropertyDisplayName("露光補正量")]
        public double? ExposureBiasValue { get { return _ExposureBiasValue; } }
        [Category("撮影項目"), Description("レンズ開放F値を表します。"), PropertyDisplayName("レンズ開放F値")]
        public double? MaxApertureValue { get { return _MaxApertureValue; } }
        [Category("撮影項目"), Description("被写体距離を表します。"), PropertyDisplayName("被写体距離")]
        public double? SubjectDistance { get { return _SubjectDistance; } }
        [Category("撮影項目"), Description("自動露出測光モードを表します。"), PropertyDisplayName("自動露出測光モード")]
        public string MeteringMode { get { return _MeteringMode; } }
        [Category("撮影項目"), Description("光源を表します。"), PropertyDisplayName("光源")]
        public string LightSource { get { return _LightSource; } }
        [Category("撮影項目"), Description("フラッシュ状態を表します。"), PropertyDisplayName("フラッシュ状態")]
        public ushort? Flash { get { return _Flash; } }
        [Category("撮影項目"), Description("レンズ焦点距離を表します。"), PropertyDisplayName("レンズ焦点距離")]
        public double? FocasLength { get { return _FocasLength; } }
        [Category("撮影項目"), Description("メーカー依存データを表します。"), PropertyDisplayName("メーカ依存データ")]
        public string MakerNote { get { return _MakerNote; } }
        [Category("撮影項目"), Description("ユーザコメントを表します。"), PropertyDisplayName("ユーザコメント")]
        public string UserComment { get { return _UserComment; } }
        [Category("撮影項目"), Description("連続撮影開始時間を表します。"), PropertyDisplayName("連続撮影時間")]
        public string SubsecTime { get { return _SubsecTime; } }
        [Category("撮影項目"), Description("連続撮影時間小数点以下を表します。"), PropertyDisplayName("連続撮影時間小数点以下")]
        public string SubsecTimeOriginal { get { return _SubsecTimeOriginal; } }
        [Category("撮影項目"), Description("連続撮影デジタル化時間小数点以下を表します。"), PropertyDisplayName("連続撮影デジタル化時間小数点以下")]
        public string SubsecTimeDigitized { get { return _SubsecTimeDigitized; } }
        [Category("撮影項目"), Description("フラッシュピックスバージョンを表します。"), PropertyDisplayName("フラッシュピックスバージョン")]
        public string FlashPixVersion { get { return _FlashPixVersion; } }
        [Category("画像情報"), Description("使用色空間を表します。"), PropertyDisplayName("使用色空間")]
        public string ColorSpace { get { return _ColorSpace; } }
        [Category("画像情報"), Description("メイン画像の幅を表します。"), PropertyDisplayName("メイン画像の幅")]
        public uint? ExifImageWidth { get { return _ExifImageWidth; } }
        [Category("画像情報"), Description("メイン画像の高さを表します。"), PropertyDisplayName("メイン画像の高さ")]
        public uint? ExifImageHeight { get { return _ExifImageHeight; } }
        [Category("画像情報"), Description("音声ファイル場所を表します。"), PropertyDisplayName("音声ファイルパス")]
        public string RelatedSoundFile { get { return _RelatedSoundFile; } }
        [Category("内部項目"), Description("拡張データオフセット値を表します。"), PropertyDisplayName("InteroperabilityIFDPointer")]
        public uint? InteroperabilityIFDPointer { get { return _InteroperabilityIFDPointer; } }
        [Category("画像情報"), Description("CCD位置画素密度幅を表します。"), PropertyDisplayName("CCD位置画素密度幅")]
        public string FocalPlaneXResolution { get { return _FocalPlaneXResolution; } }
        [Category("画像情報"), Description("CCD位置画素密度高さを表します。"), PropertyDisplayName("CCD位置画素密度高さ")]
        public string FocalPlaneYResolution { get { return _FocalPlaneYResolution; } }
        [Category("画像情報"), Description("CCD位置画素密度単位を表します。"), PropertyDisplayName("CCD位置画素密度単位")]
        public string FocalPlaneResolutionUnit { get { return _FocalPlaneResolutionUnit; } }
        [Category("撮影項目"), Description("CCD感度を表します。"), PropertyDisplayName("CCD感度")]
        public string ExposureIndex { get { return _ExposureIndex; } }
        [Category("撮影項目"), Description("CCD感度を表します。"), PropertyDisplayName("CCD感度")]
        public string SensingMethod { get { return _SensingMethod; } }
        [Category("画像情報"), Description("画像ソースを表します。"), PropertyDisplayName("画像ソース")]
        public string FileSource { get { return _FileSource; } }
        [Category("撮影項目"), Description("撮影タイプを表します。"), PropertyDisplayName("撮影タイプ")]
        public string SceneType { get { return _SceneType; } }
        [Category("撮影項目"), Browsable(false)]
        public string CFAPattern { get { return _CFAPattern; } }


        #endregion // Property

        private void GetDictionaly() {
            OrientationValueDict.Add(1,"Top Left");
            OrientationValueDict.Add(2,"Top Right");
            OrientationValueDict.Add(3,"Bottom Right");
            OrientationValueDict.Add(4,"Bottom Left");
            OrientationValueDict.Add(5,"Left Top");
            OrientationValueDict.Add(6,"Right Top");
            OrientationValueDict.Add(7,"Right Bottom");
            OrientationValueDict.Add(8,"Left Bottom");

            ExposureProgramValueDict.Add(1, "マニュアル設定");
            ExposureProgramValueDict.Add(2, "プログラムAE設定");
            ExposureProgramValueDict.Add(3, "絞り優先設定");
            ExposureProgramValueDict.Add(4, "シャッター速度優先設定");
            ExposureProgramValueDict.Add(5, "低速プログラム設定");
            ExposureProgramValueDict.Add(6, "高速プログラム設定");
            ExposureProgramValueDict.Add(7, "ポートレート設定");
            ExposureProgramValueDict.Add(8, "風景モード設定");

            MeteringModeValueDict.Add(0, "不明");
            MeteringModeValueDict.Add(1, "平均測光");
            MeteringModeValueDict.Add(2, "中央重点測光");
            MeteringModeValueDict.Add(3, "単一スポット測光");
            MeteringModeValueDict.Add(4, "多点スポット測光");
            MeteringModeValueDict.Add(5, "マルチセグメント測光");
            MeteringModeValueDict.Add(6, "部分測光");
            MeteringModeValueDict.Add(255, "その他測光");

            LightSourceValueDict.Add(0,"不明光源");
            LightSourceValueDict.Add(1, "昼光光源");
            LightSourceValueDict.Add(2, "蛍光灯光源");
            LightSourceValueDict.Add(3, "白熱電球光源");
            LightSourceValueDict.Add(10, "フラッシュ光源");
            LightSourceValueDict.Add(17, "標準ライトA光源");
            LightSourceValueDict.Add(18, "標準ライトB光源");
            LightSourceValueDict.Add(19, "標準ライトC光源");
            LightSourceValueDict.Add(20, "D55");
            LightSourceValueDict.Add(21, "D65");
            LightSourceValueDict.Add(22, "D75");
            LightSourceValueDict.Add(255, "その他光源");

            FlashValueDict.Add(0, "非発光");
            FlashValueDict.Add(1, "発光(反射光検出機構なし)");
            FlashValueDict.Add(5, "発光(反射光検出非認識)");
            FlashValueDict.Add(7, "発光(反射光検出認識)");
            FlashValueDict.Add(255, "発光(反射光検出認識)");

            ColorSpaceValueDict.Add(1,"sRGB");
            ColorSpaceValueDict.Add(65535, "Uncalibrated");

            FocalPlaneResolutionUnitValueDict.Add(1, "単位なし");
            FocalPlaneResolutionUnitValueDict.Add(2, "インチ");
            FocalPlaneResolutionUnitValueDict.Add(3, "センチメートル");
            FocalPlaneResolutionUnitValueDict.Add(255, "不明");

            SensingMethodValueDict.Add(1, "未定義");
            SensingMethodValueDict.Add(2, "1チップカラーエリアセンサ");
            SensingMethodValueDict.Add(3, "2チップカラーエリアセンサ");
            SensingMethodValueDict.Add(4, "3チップカラーエリアセンサ");
            SensingMethodValueDict.Add(5, "カラーシークエンシャルエリアセンサ");
            SensingMethodValueDict.Add(6, "未定義");
            SensingMethodValueDict.Add(7, "トリリニアセンサー");
            SensingMethodValueDict.Add(8, "カラーシークエンシャルリニアセンサ");
            SensingMethodValueDict.Add(255, "不明");
        }


        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="path"> 画像パス </param>
        public Exif(string path) {
            try {
                _Image = new Bitmap(path);

                FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
                byte[] jpegByte = new byte[fs.Length];
                fs.Read(jpegByte, 0, jpegByte.Length);
                fs.Close();

                // Jpegチェック
                byte[] maker0Byte = CheckJPEG(jpegByte);
                if (jpegByte == null)
                    return;

                // App0,APP1の確認
                byte[] tiffByte;
                if ((((maker0Byte[0] << 8) | maker0Byte[1]) == 0xffe0)) {
                    tiffByte = GetAPP1(GetAPP0(maker0Byte));
                } else {
                    tiffByte = GetAPP1(maker0Byte);
                }

                // TiffHeader 取得
                GetDictionaly();        // ディクショナリ作成
                GetTiffHeader(tiffByte);    // Tiffヘッダ取得
            } catch (Exception exp) {
                throw exp;
            }
        }


        /// <summary>
        /// 入力データがJPEGかどうか判別し、Exifヘッダを外した値を返します。
        /// (* デフォルトでは、データ入力時に必ず行うこと)
        /// </summary>
        /// <param name="data"> 入力データ </param>
        /// <returns> jpegであれば、以降の値を返す。 jpeg出なければnull値 </returns>
        private byte[] CheckJPEG(byte[] data) {
            try {
                if (!(((data[0] << 8) | data[1]) == 0xffd8))    // FFD8を確認
                    return null;
                _IsJpeg = true;

                int rtnSize = data.Length - 2;
                byte[] rtnByte = new byte[rtnSize];
                unsafe {
                    fixed (byte* p = &data[2]) {
                        Marshal.Copy(new IntPtr(p), rtnByte, 0, rtnSize * sizeof(byte));
                    }
                }

                return rtnByte;
            } catch (Exception exp) {
                return null;
            }
        }

        /// <summary>
        /// APP0の値を取得し、APP0領域を外した値を返します。
        /// </summary>
        /// <param name="data"> 入力データ </param>
        /// <returns> 正常に取得できれば、以降の値を返す。 異常であればnull値 </returns>
        private byte[] GetAPP0(byte[] data) {
            try {
                // 一応 0xFFE0をチェック
                if (!(((data[0] << 8) | data[1]) == 0xffe0))    // 0xFFE0(APP0マーカ)を確認
                    return data;
                _ContainApp0 = true;

                int sizeApp0 = (data[2] << 8 | data[3]);            // APP1のサイズ取得

                if (!(data[4] == Convert.ToInt32('J') &&             // "Exif"確認
                     data[5] == Convert.ToInt32('F') &&
                     data[6] == Convert.ToInt32('I') &&
                     data[7] == Convert.ToInt32('F')))
                    return null;

                int JFIFversion = (data[9] << 8 | data[10]);
                int Aspect = data[11];
                int Xd = (data[12] << 8 | data[13]);
                int Yd = (data[14] << 8 | data[15]);
                int Xs = data[16];
                int Ys = data[17];

                int rtnSize = data.Length - (sizeApp0 + 2);         // **** 注意 APP1はヘッダの値は入らない！
                byte[] rtnByte = new byte[rtnSize];
                unsafe {
                    fixed (byte* p = &data[sizeApp0 + 2]) {
                        Marshal.Copy(new IntPtr(p), rtnByte, 0, rtnSize * sizeof(byte));
                    }
                }
                return rtnByte;
            } catch (Exception exp) {
                return null;
            }
        }


        /// <summary>
        /// App0の読み込み
        /// </summary>
        /// <returns> Tiff ファイルフォーマットバイト配列 </returns>
        private byte[] GetAPP1(byte[] data) {
            try {
                if (!(((data[0] << 8) | data[1]) == 0xffe1))    // ffe1(APP1マーカ)を確認
                    return data;
                _ContainApp1 = true;

                int sizeApp1 = (data[2] << 8 | data[3]);            // APP1のサイズ取得

                if ((data[4] == Convert.ToInt32('E') &&             // "Exif"確認
                    data[5] == Convert.ToInt32('x') &&
                    data[6] == Convert.ToInt32('i') &&
                    data[7] == Convert.ToInt32('f')))
                    _IsExif = true;


                int rtnSize = data.Length - (10);             // **** 注意 APP1は固定値とする
                byte[] rtnByte = new byte[rtnSize];
                unsafe {
                    fixed (byte* p = &data[10]) {
                        Marshal.Copy(new IntPtr(p), rtnByte, 0, rtnSize * sizeof(byte));
                    }
                }

                return rtnByte;
            } catch (Exception exp) {
                return null;
            }

        }


        /// <summary>
        /// Tiffヘッダ読み込み
        /// </summary>
        /// <param name="app1ByteArr"></param>
        /// <returns></returns>
        private bool GetTiffHeader(byte[] app1ByteArr) {
            // Tiff IFDサーチ
            bool isIntelFormat = true;
            int tiffOffset =0;
            // ファイル形式読み込み(0 - 2 byte)
            for (int i = 0; i < app1ByteArr.Length - 1; i++) {
                int tag = ((app1ByteArr[i] << 8) | app1ByteArr[i + 1]);
                if (tag == 0x4949) {        // intel 形式
                    tiffOffset = i;
                    break;
                }

                if (tag == 0x4d4d) {        // motoroler 形式
                    isIntelFormat = false;
                    tiffOffset = i;
                    break;
                }
            }

            // check tif
            int chktag = (app1ByteArr[tiffOffset + 2] << 8) | app1ByteArr[tiffOffset + 3];
            if (isIntelFormat) {
                if (!(chktag == 0x2a00))        // intelフォーマットで 0x002Aでない場合
                    return false;
            } else {
                if (!(chktag == 0x002a))        // motorolerフォーマットで 0x002Aでない場合
                    return false;
            }

            // Tiff byte 作成
            byte[] tiffBytesAfter002A = new byte[app1ByteArr.Length - tiffOffset];
            unsafe {
                fixed (byte* p = &app1ByteArr[tiffOffset]) {
                    Marshal.Copy(new IntPtr(p), tiffBytesAfter002A, 0, (app1ByteArr.Length - tiffOffset) * sizeof(byte));
                }
            }

            // ヘッダ読み込み
            if (isIntelFormat) {
                GetTiffTagIntel(tiffBytesAfter002A);
                GetExifTagIntel(tiffBytesAfter002A, (uint)_ExifIFDPinter);
            } else {
                GetTiffTagMotoroler(tiffBytesAfter002A);
            }

            return true;
        }




        /// <summary>
        /// Tiff フォーマットヘッダ読み込み(IFD0)
        /// </summary>
        /// <param name="tiffBytesAfter002A"> tiffファイルフォーマットバイト配列　</param>
        private void GetTiffTagIntel(byte[] tiffBytesAfter002A) {
            // First Tiff Tag Offset 取得
            uint firstTiffTagOffset = GetuInt32IntelByte(tiffBytesAfter002A, 4);        // 4byte目から4byte
            // Directory Entry 数取得
            uint dirEntCount = GetuShort16IntelByte(tiffBytesAfter002A, (int)firstTiffTagOffset);  // First tiff tag offset から2byte
            uint dirEntOffset = firstTiffTagOffset + 2;
            // 各Tag要素の取得
            for (int i = 0; i < dirEntCount; i++) {
                int offset = (int)(dirEntOffset + 12 * i);
                ushort tag = GetuShort16IntelByte(tiffBytesAfter002A, offset);          // tag 2byte
                ushort format = GetuShort16IntelByte(tiffBytesAfter002A, offset + 2);   // データ種類 2byte
                uint count = GetuInt32IntelByte(tiffBytesAfter002A, offset + 4);    // データ個数 4byte
                uint pointer = GetuInt32IntelByte(tiffBytesAfter002A, offset + 8);  // データアドレス 4byte
                GetIFD0Data(tiffBytesAfter002A, tag, format, count, pointer);
            }
        }

        /// <summary>
        /// Tiff フォーマットヘッダ値振り分け(IFD0)
        /// </summary>
        /// <param name="refByte"> tiffファイルフォーマットバイト配列 </param>
        /// <param name="tag"> タグ </param>
        /// <param name="format"> フォーマット </param>
        /// <param name="count"> データ数 </param>
        /// <param name="pointer"> ポインタ </param>
        private void GetIFD0Data(byte[] refByte, ushort tag, ushort format, uint count, uint pointer) {
            // タグのデータを取得
            byte[] data = GetTagData(refByte, format, count, pointer);
            // 各タグにより所望の値に振り分け
            switch (tag) {
                case 0x010e:
                    _ImageDescription = GetStringIntelByte(data, 0, data.Length);   // char[]
                    break;
                case 0x010f:
                    _Make = GetStringIntelByte(data, 0, data.Length);               // char[]
                    break;
                case 0x0110:
                    _Model = GetStringIntelByte(data, 0, data.Length);              // char[]
                    break;
                case 0x0112:
                    ushort orientation = GetuShort16IntelByte(data, 0);             // 2byte
                    if (OrientationValueDict.ContainsKey(orientation))
                        _Orientation = OrientationValueDict[orientation];
                    break;
                case 0x011a:
                    _XResolution = GetFractionString(data, (int)format);            // 8byte
                    break;
                case 0x011b:
                    _YResolution = GetFractionString(data, (int)format);            // 8byte
                    break;
                case 0x0128:
                    _ResolutionUnit = GetuShort16IntelByte(data, 0);                // 2byte
                    break;
                case 0x0131:
                    _Software = GetStringIntelByte(data, 0, data.Length);           // char[]
                    break;
                case 0x0132:
                    _Date = GetDateTimeFromString(GetStringIntelByte(data, 0, data.Length));    // char[] (DateTime)
                    break;
                case 0x013e:
                    _WhitePoint = GetFractionString(data, (int)format);             // 8byte
                    break;
                case 0x013f:
                    _PrimaryChromaticities = GetFractionString(data, (int)format);  // 8byte
                    break;
                case 0x0211:
                    _YCbCrCoefficients = GetFractionString(data, (int)format);      // char[]
                    break;
                case 0x0213:
                    _YCbCrPositioning = GetuShort16IntelByte(data, 0);              // 2byte
                    break;
                case 0x0214:
                    _ReferenceBlackWhite = GetFractionString(data, (int)format);    // char[]  
                    break;
                case 0x8298:
                    _Copyright = GetStringIntelByte(data, 0, data.Length);          // char[]
                    break;
                case 0x8769:
                    _ExifIFDPinter = GetuInt32IntelByte(data, 0);                   // 4byte
                    break;

            }
        }


        /// <summary>
        /// Tiff フォーマットExifヘッダ読み込み(IFD0)
        /// </summary>
        /// <param name="tiffBytesAfter002A"> tiffファイルフォーマットバイト配列</param>
        /// <param name="startAddress"> オフセット </param>
        private void GetExifTagIntel(byte[] tiffBytesAfter002A , uint startAddress) {
            try {
                // Directory Entry 数取得
                uint dirEntCount = GetuShort16IntelByte(tiffBytesAfter002A, (int)startAddress);  // exif tag offset から2byte
                uint dirEntOffset = startAddress + 2;
                // 各Tag要素の取得
                for (int i = 0; i < dirEntCount; i++) {
                    int offset = (int)(dirEntOffset + 12 * i);
                    ushort tag = GetuShort16IntelByte(tiffBytesAfter002A, offset);          // tag 2byte
                    ushort format = GetuShort16IntelByte(tiffBytesAfter002A, offset + 2);   // データ種類 2byte
                    uint count = GetuInt32IntelByte(tiffBytesAfter002A, offset + 4);    // データ個数 4byte
                    uint pointer = GetuInt32IntelByte(tiffBytesAfter002A, offset + 8);  // データアドレス 4byte
                    GetExifSubIFDData(tiffBytesAfter002A, tag, format, count, pointer);
                }
            } catch (Exception exp) { 
            
            }
        }

        /// <summary>
        /// Tiff フォーマットExifヘッダ値振りわけ(IFD0)
        /// </summary>
        /// <param name="refByte"> tiffファイルフォーマットバイト配列 </param>
        /// <param name="tag"> タグ </param>
        /// <param name="format"> フォーマット </param>
        /// <param name="count"> データ数 </param>
        /// <param name="pointer"> ポインタ </param>
        private void GetExifSubIFDData(byte[] refByte, ushort tag, ushort format, uint count, uint pointer) {
            try {
                // タグのデータを取得
                byte[] data = GetTagData(refByte, format, count, pointer);

                // 各タグにより所望の値に振り分け
                switch (tag) {
                    case 0x829a:
                        _ExposureTime = GetFractionString(data, format);
                        break;
                    case 0x829d:
                        _FNumber = GetFractionString(data, format);
                        break;
                    case 0x8822:
                        ushort exposureProgram = GetuShort16IntelByte(data, 0);
                        if (ExposureProgramValueDict.ContainsKey(exposureProgram))
                            _ExposureProgram = ExposureProgramValueDict[exposureProgram];
                        break;
                    case 0x8827:
                        _ISOSpeedRatings = GetuShort16IntelByte(data, 0);
                        break;
                    case 0x9000:
                        _ExifVersion = ""; // TODO
                        break;
                    case 0x9003:
                        _DateTimeOriginal = GetDateTimeFromString(GetStringIntelByte(data, 0, data.Length));
                        break;
                    case 0x9004:
                        _DateTimeDigitized = GetDateTimeFromString(GetStringIntelByte(data, 0, data.Length));
                        break;
                    case 0x9101:
                        _ComponentsConfiguration = ""; //TODO
                        break;
                    case 0x9102:
                        _CompressedBitsPerPixel = GetFractionString(data, format);
                        break;
                    case 0x9201:
                        _ShutterSpeedValue = GetFractionValue(GetFractionSignString(data, format)); 
                        break;
                    case 0x9202:
                        _ApertureValue = GetFractionValue(GetFractionString(data, format));
                        break;
                    case 0x9203:
                        _BrightnessValue = GetFractionValue(GetFractionString(data, format));
                        break;
                    case 0x9204:
                        _ExposureBiasValue = GetFractionValue(GetFractionSignString(data, format));
                        break;
                    case 0x9205:
                        _MaxApertureValue = GetFractionValue(GetFractionString(data, format));
                        break;
                    case 0x9206:
                        _SubjectDistance = GetFractionValue(GetFractionSignString(data, format));
                        break;
                    case 0x9207:
                        ushort meteringMode = GetuShort16IntelByte(data, 0);
                        if (MeteringModeValueDict.ContainsKey(meteringMode))
                            _MeteringMode = MeteringModeValueDict[meteringMode];
                        break;
                    case 0x9208:
                        ushort lightSource = GetuShort16IntelByte(data, 0);
                        if (LightSourceValueDict.ContainsKey(lightSource))
                            _LightSource = LightSourceValueDict[lightSource];
                        break;
                    case 0x9209:
                        _Flash = GetuShort16IntelByte(data, 0);
                        break;
                    case 0x920a:
                        _FocasLength = GetFractionValue(GetFractionString(data, format));
                        break;
                    case 0x927c:
                        _MakerNote = ""; // TODO
                        break;
                    case 0x9286:
                        _UserComment = ""; //TODO
                        break;
                    case 0x9290:
                        _SubsecTime = GetStringIntelByte(data, 0, data.Length);
                        break;
                    case 0x9291:
                        _SubsecTimeOriginal = GetStringIntelByte(data, 0, data.Length);
                        break;
                    case 0x9292:
                        _SubsecTimeDigitized = GetStringIntelByte(data, 0, data.Length);
                        break;
                    case 0xa000:
                        _FlashPixVersion = ""; // TODO
                        break;
                    case 0xa001:
                        ushort colorSpace = GetuShort16IntelByte(data, 0);
                        if (ColorSpaceValueDict.ContainsKey(colorSpace))
                            _ColorSpace = ColorSpaceValueDict[colorSpace];
                        break;
                    case 0xa002:
                        _ExifImageWidth = GetuInt32IntelByte(data, 0);
                        break;
                    case 0xa003:
                        _ExifImageHeight = GetuInt32IntelByte(data, 0);
                        break;
                    case 0xa004:
                        _RelatedSoundFile = GetStringIntelByte(data, 0, data.Length);
                        break;
                    case 0xa005:
                        _InteroperabilityIFDPointer = GetuInt32IntelByte(data, 0);
                        break;
                    case 0xa20e:
                        _FocalPlaneXResolution = GetFractionString(data, format);
                        break;
                    case 0xa20f:
                        _FocalPlaneYResolution = GetFractionString(data, format);
                        break;
                    case 0xa210:
                        ushort focalPlaneResolutionUnit = GetuShort16IntelByte(data, 0);
                        if (FocalPlaneResolutionUnitValueDict.ContainsKey(focalPlaneResolutionUnit))
                            _FocalPlaneResolutionUnit = FocalPlaneResolutionUnitValueDict[focalPlaneResolutionUnit];
                        break;
                    case 0xa215:
                        _ExposureIndex = GetFractionString(data, format);
                        break;
                    case 0xa217:
                        ushort sensingMethod = GetuShort16IntelByte(data, 0);
                        if (SensingMethodValueDict.ContainsKey(sensingMethod))
                            _SensingMethod = SensingMethodValueDict[sensingMethod];
                        break;
                    case 0xa300:
                        _FileSource = ""; // TODO;
                        break;
                    case 0xa301:
                        ushort scaneType = GetuShort16IntelByte(data, 0);
                        if (scaneType == 0x01)
                            _SceneType = "直接撮影";
                        else
                            _SceneType = "その他"; // TODO
                        break;
                    case 0xa302:
                        _CFAPattern = ""; // TODO
                        break;
                }

            } catch (Exception exp) { 
            
            }
        }



        #region Data Read

        /// <summary>
        /// タグデータ読み込み
        /// ポインタの値が4byte以上であれば、ポインタアドレスからデータ長を返す。
        /// 4byte以下であれば、ポインタアドレス値を返す
        /// </summary>
        /// <param name="refByte"> tiffファイルフォーマットバイト配列 </param>
        /// <param name="format"> フォーマット </param>
        /// <param name="count"> データ数 </param>
        /// <param name="pointer"> ポインタ値 </param>
        /// <returns> データ配列 </returns>
        private byte[] GetTagData(byte[] refByte, uint format, uint count, uint pointer) {
            int length = (int)(_dataLength[format] * count);
            byte[] rtnByte;
            if (length > 4) {  // データが4byteを越えるときはオフセットのアドレス(data)から取得する。
                rtnByte = new byte[length];
                unsafe {
                    fixed (byte* p = &refByte[pointer]) {
                        Marshal.Copy(new IntPtr(p), rtnByte, 0, length * sizeof(byte));
                    }
                }
            } else {
                rtnByte = BitConverter.GetBytes(pointer);
            }
            return rtnByte;
        }

        private short GetShort16IntelByte(byte[] refByte, int startIndex) {
            try {
                return BitConverter.ToInt16(refByte, startIndex);
            } catch (Exception exp) {
                return 0;
            }
        }


        private ushort GetuShort16IntelByte(byte[] refByte, int startIndex) {
            try {
                return BitConverter.ToUInt16(refByte, startIndex);
            } catch (Exception exp) {
                return 0;
            }
        }

        private int GetInt32IntelByte(byte[] refByte, int startIndex) {
            try {
                return BitConverter.ToInt32(refByte, startIndex);
            } catch (Exception exp) {
                return 0;
            }
        }

        private uint GetuInt32IntelByte(byte[] refByte, int startIndex) {
            try {
                return BitConverter.ToUInt32(refByte, startIndex);
            } catch (Exception exp) {
                return 0;
            }
        }

        private int GetIntIntelByte(byte[] refByte, int startIndex, int length) {
            try {
                int rtnInt = 0;
                for (int i = 0; i < length; i++)
                    rtnInt = rtnInt | (refByte[startIndex + i] << (8 * i));

                return rtnInt;
            } catch (Exception exp) {
                return 0;
            }
        }


        private string GetStringIntelByte(byte[] refByte, int startIndex, int length) {
            try {
                string rtnString = "";
                for (int i = 0; i < startIndex + length; i++)
                    rtnString += Convert.ToChar(refByte[i]).ToString();

                return rtnString;
            } catch (Exception exp) {
                return null;
            }
        }

        private void GetTiffTagMotoroler(byte[] tiffBytesAfter002A) {


        }

        /// <summary>
        /// signed rational 読み込み
        /// </summary>
        /// <param name="data"> データバイト配列</param>
        /// <param name="format"> フォーマット </param>
        /// <returns> unsigned rational　文字列 </returns>
        private string GetFractionSignString(byte[] data, int format) {
            int Nume = GetInt32IntelByte(data, 0);
            int Deno = GetInt32IntelByte(data, _dataLength[format] / 2);
            return Nume.ToString() + "/" + Deno.ToString();
        }

        /// <summary>
        /// unsigned rational　読み込み
        /// </summary>
        /// <param name="data"> データバイト配列</param>
        /// <param name="format"> フォーマット </param>
        /// <returns> unsigned rational　文字列 </returns>
        private string GetFractionString(byte[] data, int format) {
            uint Nume = GetuInt32IntelByte(data, 0);
            uint Deno = GetuInt32IntelByte(data, _dataLength[format] / 2);
            return Nume.ToString() + "/" + Deno.ToString();
        }

        /// <summary>
        /// GetFractionString,GetFractionSignStringで得たstring フォーマットをdouble に変換
        /// </summary>
        /// <param name="value">GetFractionString,GetFractionSignStringで得たstring フォーマット</param>
        /// <returns> 変換値 </returns>
        private double GetFractionValue(string value) {
            string[] strings = value.Split('/');
            return double.Parse(strings[0]) / double.Parse(strings[1]);
        }


        /// <summary>
        /// ascii strings 時間変更
        /// </summary>
        /// <param name="datetimestring"> ascii strings 文字列 </param>
        /// <returns> 時間</returns>
        private DateTime GetDateTimeFromString(string datetimestring) {
            string[] sep = datetimestring.Split(' ');
            string[] sepDate = sep[0].Split(':');
            string[] sepTime = sep[1].Split(':');

            return new DateTime(int.Parse(sepDate[0]), int.Parse(sepDate[1]), int.Parse(sepDate[2]),
                int.Parse(sepTime[0]), int.Parse(sepTime[1]), int.Parse(sepTime[2]));
        }
        #endregion
    }
}
