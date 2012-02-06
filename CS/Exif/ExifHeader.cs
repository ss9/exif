using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Runtime.InteropServices;
using System.ComponentModel;

namespace ImageLib_ss {
    public class ExifHeader {

        // 取得値に対する文字列ディクショナリ
        public Dictionary<int, string> OrientationValueDict = new Dictionary<int, string>();
        public Dictionary<int, string> ExposureProgramValueDict = new Dictionary<int, string>();
        public Dictionary<int, string> MeteringModeValueDict = new Dictionary<int, string>();
        public Dictionary<int, string> LightSourceValueDict = new Dictionary<int, string>();
        public Dictionary<int, string> FlashValueDict = new Dictionary<int, string>();
        public Dictionary<int, string> ColorSpaceValueDict = new Dictionary<int, string>();
        public Dictionary<int, string> FocalPlaneResolutionUnitValueDict = new Dictionary<int, string>();
        public Dictionary<int, string> SensingMethodValueDict = new Dictionary<int, string>();

        private Bitmap _Image;

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
        private DateTime _Date;
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

        [Category("Image"), Description("イメージのプロパティを表します。")]
        public Bitmap Image { get { return _Image; } }


        // header プロパティ
        [Category("内部項目"), Description("JPEGの可否を表します。"), PropertyDisplayName("Jpeg?")]
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



        #region Function



        /// <summary>
        /// 取得値に対する名称ディクショナリ作成
        /// </summary>
        private void GetDictionaly() {
            OrientationValueDict.Add(1, "Top Left");
            OrientationValueDict.Add(2, "Top Right");
            OrientationValueDict.Add(3, "Bottom Right");
            OrientationValueDict.Add(4, "Bottom Left");
            OrientationValueDict.Add(5, "Left Top");
            OrientationValueDict.Add(6, "Right Top");
            OrientationValueDict.Add(7, "Right Bottom");
            OrientationValueDict.Add(8, "Left Bottom");

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

            LightSourceValueDict.Add(0, "不明光源");
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

            ColorSpaceValueDict.Add(1, "sRGB");
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

        #endregion // Function

    }
}
