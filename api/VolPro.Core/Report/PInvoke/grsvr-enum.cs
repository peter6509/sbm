//grsvr-enum.cs: 由锐浪報表接口维护工具自動生成，請勿手動修改
//生成時间: 2023/03/09 11:26:41


namespace gridreport
{

public enum AlignColumnStyle
{
    Left = 1, //對齐到列的左端。
    Right = 2, //對齐到列的右端。
    Both = 3, //對齐到列的左右两端，部件框宽度與列保持一致。
}

public enum AnchorStyles
{
    Left = 1, //该部件框锚定到其容器的左边缘。
    Top = 2, //该部件框锚定到其容器的上边缘。
    Right = 4, //该部件框锚定到其容器的右边缘。
    Bottom = 8, //该部件框锚定到其容器的下边缘。
}

public enum BackStyle
{
    Normal = 1, //部件框使用背景色填充他占據的矩形区域。
    Transparent = 2, //部件框背景透明，允许任何背景图像或其他部件框透過自身顯示。
}

public enum BarcodeCaptionPosition
{
    None = 1, //条形碼的文字不顯示。
    Above = 2, //条形碼的文字顯示在上端。
    Below = 3, //条形碼的文字顯示在下端。
}

public enum BarcodeDirection
{
    LeftToRight = 1, //从左到右。
    RightToLeft = 2, //从右到左。
    TopToBottom = 3, //从上到下。
    BottomToTop = 4, //从下到上。
}

public enum BarcodeType
{
    Code25Intlv = 1, //Code25 Interleaved
    Code25Ind = 2, //Code25 Industrial,
    Code25Matrix = 3, //Code25 Matrix,
    Code39 = 4, //Code39
    Code39X = 5, //Code39X
    Code128A = 6, //Code128A
    Code128B = 7, //Code128B
    Code128C = 8, //Code128C
    Code93 = 9, //Code93
    Code93X = 10, //Code93 Extended
    CodeMSI = 11, //CodeMSI
    CodePostNet = 12, //CodePostNet
    CodeCodabar = 13, //CodeCodabar
    CodeEAN8 = 14, //CodeEAN8，商品碼，數據只能為數字
    CodeEAN13 = 15, //CodeEAN13，，商品碼，數據只能為數字。如果進行數據校驗，提供12位數字數據，否则13位數字數據
    CodeUPC_A = 16, //CodeUPC_A，商品碼，數據只能為數字
    CodeUPC_E0 = 17, //CodeUPC_E0，商品碼，數據只能為數字
    CodeUPC_E1 = 18, //CodeUPC_E1，商品碼，數據只能為數字
    CodeUPC_Supp2 = 19, //CodeUPC_Supp2
    CodeUPC_Supp5 = 20, //CodeUPC_Supp5
    CodeEAN128A = 21, //CodeEAN128A
    CodeEAN128B = 22, //CodeEAN128B
    CodeEAN128C = 23, //CodeEAN128C
    Code128 = 24, //Code128
    GS1Code128 = 25, //GS1 Code128(EAN128)
    ITF_14 = 26, //ITF_14
    PDF417 = 50, //二维碼PDF417
    QRCode = 51, //二维碼QRCode
    DataMatrix = 52, //二维碼DataMatrix
    GS1DataMatrix = 53, //GS1 DataMatrix，以GS1標准编碼數據生成 DataMatrix 碼
    GS1QRCode = 54, //GS1 QRCode，以GS1標准编碼數據生成 QRCode 碼
}

public enum BorderStyles
{
    DrawLeft = 1, //部件框或明细网格顯示左边边框線。
    DrawTop = 2, //部件框或明细网格顯示上边边框線。
    DrawRight = 4, //部件框或明细网格顯示右边边框線。
    DrawBottom = 8, //部件框或明细网格顯示下边边框線。
}

public enum CenterStyle
{
    None = 0, //部件框在父容器中不居中對齐。
    Horizontal = 1, //部件框始终位于父容器水平方向的正中间。
    Vertical = 2, //部件框始终位于父容器垂直方向的正中间。
    Both = 3, //部件框始终位于父容器水平與垂直方向的正中间
}

public enum ChartType
{
    BarChart = 1, //柱图。
    PieChart = 2, //饼图。
    LineChart = 3, //折線图。
    StackedBarChart = 4, //叠加柱图
    XYScatterChart = 5, //散列點图
    XYLineChart = 6, //散列點連線图
    CurveLineChart = 7, //連曲線图
    XYCurveLineChart = 8, //散列點連曲線图
    Bubble = 9, //气泡图
    StackedBar100Chart = 10, //百分比柱状图
    ColumnChart = 11, //横向柱状图
    StackedColumnChart = 12, //横向叠加柱状图
    StackedColumn100Chart = 13, //横向百分比柱状图
}

public enum ChartVarType
{
    XVal = 1, //X值
    YVal = 2, //Y值
    ZVal = 3, //Z值
    YVal100ByGroup = 4, //Y值在當前组簇中的百分比
    YVal100BySeries = 5, //Y值在當前序列中的百分比
    YValTotalByGroup = 6, //當前组簇中的Y值合计
    YValTotalBySeries = 7, //當前序列中的Y值合计
    SeriesLabel = 8, //當前序列中的標签文字
    GroupLabel = 9, //當前组簇中的標签文字
}

public enum CollateKind
{
    Default = 1, //由打印機确定是否逐份打印
    Collate = 2, //逐份打印
    NotCollate = 3, //非逐份打印
}

public enum ColumnPrintAdaptMethod
{
    Discard = 1, //在打印输出時，超出页面输出范围的列内容將被忽略掉，即不输出顯示。
    Wrap = 2, //在打印输出時，超出页面输出范围的列内容將另起新行输出顯示。
    ResizeToFit = 3, //在打印输出時，根據列的宽度按比列將所有要输出的列的总宽度調整到页面输出区域的宽度。
    ShrinkToFit = 4, //在打印输出時，如果列的总宽度超出页面输出范围，與grcpamResizeToFit相同，反之按設计時宽度输出。
    ToNewPage = 5, //在打印输出時，超出页面输出范围的列内容將另起新页输出顯示，按先从上到下的顺序输出。
    ToNewPageEx = 6, //在打印输出時，超出页面输出范围的列内容將另起新页输出顯示，按先从左到右的顺序输出。
    ToNewPageRFC = 7, //在打印输出時，超出页面输出范围的列内容將另起新页输出顯示，且左边的固定列在每页中重复输出，按先从上到下的顺序输出。
    ToNewPageRFCEx = 8, //在打印输出時，超出页面输出范围的列内容將另起新页输出顯示，且左边的固定列在每页中重复输出，按先从左到右的顺序输出。
    WrapExcludeGroup = 9, //類同 grcpamWrap，但分组頭尾不另起新行。
}

public enum ControlType
{
    StaticBox = 1, //静態文字框。
    ShapeBox = 2, //图形框。
    SystemVarBox = 3, //系统变量框。
    FieldBox = 4, //字段框。
    SummaryBox = 5, //统计框。
    RichTextBox = 6, //RTF格式文字框
    PictureBox = 7, //图像框。
    MemoBox = 8, //综合文字框。
    SubReport = 9, //子報表。
    Line = 10, //線段。
    Chart = 11, //图表。
    Barcode = 12, //条形碼。
    FreeGrid = 13, //自由表格。
}

public enum DisplayCursor
{
    Default = 0, //默認光標。
    Arrow = 1, //普通箭頭光標。
    Magnify = 2, //方大镜光標。
    Finger = 3, //手指光標。
    Affirm = 4, //肯定光標。
    Negative = 5, //否定光標。
}

public enum DockStyle
{
    None = 0, //该部件框未停靠。
    Left = 1, //该部件框的左边缘停靠在父容器的左边缘。
    Top = 2, //该部件框的上边缘停靠在父容器的顶端。
    Right = 3, //该部件框的右边缘停靠在父容器的右边缘。
    Bottom = 4, //该部件框的下边缘停靠在父容器的底部。
    Fill = 5, //部件框的各個边缘分别停靠在父容器的各個边缘，並且适當調整大小。
}

public enum DocType
{
    Object = 1, //按對象方式保存模板數據，此种格式為 Grid++Report 自有格式，内容非常直观易理解。
    JSON = 2, //按 JSON 格式保存報表模板數據，完全符合 JSON 规范要求。
    Register = 3, //報表模板的WEB報表注册加密格式。
}

public enum DrawRotation
{
    Rotate0 = 0, //不旋转。
    Rotate90 = 1, //旋转90度。
    Rotate180 = 2, //旋转180度。
    Rotate270 = 3, //旋转270度。
}

public enum DtmxEncoding
{
    Auto = 2, //由程序根據數據自動选择编碼方式
    Ascii = 3, //Ascii编碼方式
    C40 = 4, //C40编碼方式
    Text = 5, //Text编碼方式
    X12 = 6, //X12编碼方式
    Edifact = 7, //Edifact编碼方式
    Base256 = 8, //Base256编碼方式
}

public enum DtmxMatrixSize
{
    Auto = 2, //由程序根據數據量自動选择
    _10x10 = 4,
    _12x12 = 5,
    _14x14 = 6,
    _16x16 = 7,
    _18x18 = 8,
    _20x20 = 9,
    _22x22 = 10,
    _24x24 = 11,
    _26x26 = 12,
    _32x32 = 13,
    _36x36 = 14,
    _40x40 = 15,
    _44x44 = 16,
    _48x48 = 17,
    _52x52 = 18,
    _64x64 = 19,
    _72x72 = 20,
    _80x80 = 21,
    _88x88 = 22,
    _96x96 = 23,
    _104x104 = 24,
    _120x120 = 25,
    _132x132 = 26,
    _144x144 = 27,
    _8x18 = 28,
    _8x32 = 29,
    _12x26 = 30,
    _12x36 = 31,
    _16x36 = 32,
    _16x48 = 33,
}

public enum DuplexKind
{
    Default = 0, //打印機默認的双面打印設置。
    Simplex = 1, //單面打印。
    Vertical = 2, //双面垂直打印。
    Horizontal = 3, //双面水平打印。
}

public enum FieldType
{
    String = 1, //字符字段。
    Integer = 2, //整數字段。
    Float = 3, //浮點數字段。
    Currency = 4, //货币字段。
    Boolean = 5, //布尔字段。
    DateTime = 6, //日期時间字段。
    Binary = 7, //二進制字段。
}

public enum GradientMode
{
    None = 0, //不應用渐变。 
    Center = 1, //渐变是从中心向外應用的。 
    LeftRight = 2, //渐变是从左向右應用的。
    TopBottom = 3, //渐变是从上到下應用的。 
    HorizontalCenter = 4, //渐变是沿水平方向从中心向外應用的。 
    VerticalCenter = 5, //渐变是沿垂直方向从中心向外應用的。 
    DiagonalLeft = 6, //渐变是沿對角方向从左向右應用的。
    DiagonalRight = 7, //渐变是沿對角方向从右向左應用的。
}

public enum GrpKpTogetherStyle
{
    None = 1, //不要求分组頭行與本分组项其他行打印输出聚集在相同页。
    FirstDetail = 2, //要求分组頭行與本分组项的第一個明细記錄行打印输出在相同页。
    All = 3, //要求分组頭行與本分组项的其他所有行尽量打印输出在相同页。
}

public enum LockType
{
    None = 0, //不锁定
    Inherited = 1, //继承：从父级對象继承锁定設置状態
    Layout = 2, //布局：不能修改布局相关的屬性設置，“布局”類别的屬性不允许修改
    Object = 3, //對象：對象本身不允许删除，不允许增加或删除其子對象
    Data = 4, //數據：在 Object 限定基础上，數據：不能修改數據相关的屬性設置，“數據”類别的屬性不允许修改
    DataAction = 5, //數據行為：在 Data 限定基础上，不能修改“行為”相关的屬性設置，“行為”與“脚本”類别的屬性不允许修改
    All = 10, //全部：任何修改都不允许，包括外观與布局相关設置都不允许改变
}

public enum NewPageColumnStyle
{
    None = 1, //分组頭在任何時候都不另起新页栏。
    Before = 2, //分组頭在打印输出之前另起新页栏進行输出。
    After = 3, //分组頭在打印输出之后另起新页栏。后續内容输出在新页栏中。
    BeforeAfter = 4, //分组頭在打印输出之前與之后都另起新页栏。
}

public enum NewPageStyle
{
    None = 1, //報表节不强制產生新页。
    Before = 2, //報表节在打印输出之前要求產生新页，保証本节在新页中输出。
    After = 3, //報表节在打印输出之后要求產生新页，保証本节之后的后續节在新页中输出。
    BeforeAfter = 4, //報表节在打印输出之前與之后要求產生新页。
}

public enum OCGroupHeaderVAlign
{
    Top = 1, //占列式分组頭顯示在整個分组列区域的顶部。
    Middle = 2, //占列式分组頭顯示在整個分组列区域的中部（垂直方向）。
    Bottom = 3, //占列式分组頭顯示在整個分组列区域的底部。
}

public enum PageColumnDirection
{
    DownAcross = 1, //按从上到下，再从左到右的顺序在页栏中打印输出。
    AcrossDown = 2, //按从左到右，再从上到下的顺序在页栏中打印输出。
    DownAcrossEqual = 3, //按从上到下，再从左到右的顺序在页栏中打印输出，保持每栏输出基本一样多的數據。
}

public enum PaperKind
{
    Letter = 1, //1/2- by 11-inches Letter，215.9 x 279.4 毫米。 
    LetterSmall = 2, //Letter Small, 8 1/2- by 11-inches 215.9 x 279.4 毫米。 
    Tabloid = 3, //11- by 17-inches Tabloid，279.4 x 431.8 毫米。 
    Ledger = 4, //17- by 11-inches 431.8 x 279.4 毫米。 
    Legal = 5, //1/2- by 14-inches Legal，215.9 x 355.6 毫米。 
    Statement = 6, //5 1/2- by 8 1/2-inches 139.7 x 215.9 毫米。 
    Executive = 7, //7 1/4- by 10 1/2-inches 184.1 x 266.7 毫米。 
    A3 = 8, //A3，297- by 420-millimeters 
    A4 = 9, //A4，210- by 297-millimeters 
    A4Small = 10, //A4 Small， 210- by 297-millimeters
    A5 = 11, //148- by 210-millimeters 
    B4 = 12, //250- by 354-millimeters 
    B5 = 13, //182- by 257-millimeter paper 
    Folio = 14, //8 1/2- by 13-inch paper 215.9 x 330.2 毫米。 
    Quarto = 15, //215- by 275-millimeter paper 
    _10X14 = 16, //by 14-inch sheet Paper10x14 纸张大小為 254 x 355.6 毫米。 
    _11X17 = 17, //17-inch sheet 纸张大小為 279.4 x 431.8 毫米。
    Note = 18, //8 1/2- by 11-inches Note，215.9 x 279.4 毫米。 
    Envelope9 = 19, //3 7/8- by 8 7/8-inches envelope（229 × 324 毫米）
    Envelope10 = 20, //4 1/8- by 9 1/2-inches #10 Envelope，104.8 x 241.3 毫米。 
    Envelope11 = 21, //4 1/2- by 10 3/8-inches #11 Envelope，114.3 x 263.5 毫米。 
    Envelope12 = 22, //4 3/4- by 11-inches #12 Envelope，120.7 x 279.4 毫米。 
    Envelope14 = 23, //5- by 11 1/2-inches #14 Envelope，127 x 292.1 毫米。 
    CSheet = 24, //17- by 22-inches C 型纸，431.8 x 558.8 毫米。 
    DSheet = 25, //22- by 34-inches D 型纸，558.8 x 863.6 毫米。  
    ESheet = 26, //E Sheet 34- by 44-inches 863.6 x 1117.6 毫米。 
    EnvelopeDL = 27, //110- by 220-millimeters 
    EnvelopeC5 = 28, //162- by 229-millimeters C5 Envelope，162 x 229 毫米。 
    EnvelopeC3 = 29, //324- by 458-millimeters C3 Envelope，324 x 458 毫米。 
    EnvelopeC4 = 30, //229- by 324-millimeters 
    EnvelopeC6 = 31, //114- by 162-millimeters 
    EnvelopeC65 = 32, //114- by 229-millimeters 
    EnvelopeB4 = 33, //250- by 353-millimeters 
    EnvelopeB5 = 34, //176- by 250-millimeters 
    EnvelopeB6 = 35, //176- by 125-millimeters 
    EnvelopeItaly = 36, //110- by 230-millimeters 110 x 230 毫米。 
    EnvelopeMonarch = 37, //3 7/8- by 7 1/2-inches 98.4 x 190.5 毫米。 
    EnvelopePersonal = 38, //3 5/8- by 6 1/2-inches 92.1 x 165.1 毫米。 
    Fanfold = 39, //14 7/8- by 11-inches 377.8 x 279.4 毫米。 
    A6 = 70, //105- by 148-millimeters 
    B6 = 88, //128- by 182-millimeters 
    _12X11 = 90, //11-inch sheet Standard 纸（305 × 279 毫米）
    P16K = 93, //PRC 16K, 146- by 215-millimeters 
    P32K = 94, //PRC 32K, 97- by 151-millimeters 
    Default = 255, //當前打印機的默認默認纸张
    Custom = 256, //用户自定義纸张，需要指定纸张的宽度與长度屬性
}

public enum PaperOrientation
{
    Default = 0, //應用打印機當前設置的纸张输出方向
    Portrait = 1, //纸张输出方向為纵向。
    Landscape = 2, //纸张输出方向為横向。
}

public enum PaperSourceKind
{
    Default = 0, //打印機默認。
    Upper = 1, //打印機的上层纸盒（如果打印機只有一個纸盒，那么這個纸盒就是上层纸盒）。
    Lower = 2, //打印機的下层纸盒。
    Middle = 3, //打印機的中层纸盒。
    Manual = 4, //以手動方式送入的纸张。
    Envelope = 5, //信封。
    ManualFeed = 6, //以手動方式送入的信封。
    AutomaticFeed = 7, //自動送入的纸张。
    TractorFeed = 8, //牵引器送纸。
    SmallFormat = 9, //小纸。
    LargeFormat = 10, //大纸。
    LargeCapacity = 11, //打印機的大容量纸盒。
    Cassette = 14, //卡式纸盒。
    FormSource = 15, //打印機的默認送纸盒。
    Custom = 256, //打印機特定的纸张来源。
}

public enum ParameterDataType
{
    String = 1, //字符串類型，可以為任意长度。
    Integer = 2, //整數類型，可以設定格式化串。
    Float = 3, //浮點數類型，可以設定格式化串。
    Boolean = 5, //布尔類型，可以真值與假值的顯示文字。
    DateTime = 6, //日期時间類型，可以設定格式化串。
}

public enum PenStyle
{
    Solid = 0, //画實線。
    Dash = 1, //画段虛線。
    Dot = 2, //画點虛線。
    DashDot = 3, //画點段虛線。
    DashDotDot = 4, //画双點段虛線。
}

public enum PeriodType
{
    None = 0, //不确定期间。
    Day = 1, //日
    Week = 2, //周
    ThirdMonth = 3, //旬
    HalfMonth = 4, //半月
    Month = 5, //月
    Quarter = 6, //季
    HalfYear = 7, //半年
    Year = 8, //年
    Calendar = 9, //日历
}

public enum PictureAlignment
{
    TopLeft = 1, //图像的左上角紧靠图像框的左上角顯示。
    TopRight = 2, //图像的右上角紧靠图像框的右上角顯示。
    Center = 3, //图像居中顯示在图像框中。
    BottomLeft = 4, //图像的左下角紧靠图像框的左下角顯示。
    BottomRight = 5, //图像的右下角紧靠图像框的右下角顯示。
}

public enum PictureRotateMode
{
    None = 0, //不旋转
    Left = 1, //左旋
    Right = 2, //右旋
    Flip = 3, //颠倒
    Mirror = 4, //镜像
}

public enum PictureSizeMode
{
    Clip = 1, //图像不進行缩放，按原始尺寸顯示。
    Stretch = 2, //图像伸缩到完全顯示到图像框中。
    Zoom = 3, //根據图像框的大小图像保持宽高比例伸缩至某一方向完全铺满。
    EnsureFullView = 4, //如果图像不能在图像框中完全顯示，根據图像框的大小图像保持宽高比例伸缩至某一方向完全铺满。反之图像保持原始尺寸顯示。
    Tile = 5, //平铺多次顯示图像，铺满整個顯示区域。
}

public enum PictureTransparentMode
{
    None = 0, //不透明
    Overlying = 1, //叠加透明
    Background = 2, //背景透明
}

public enum PictureType
{
    Unknown = 0, //未知
    BMP = 1, //BMP
    GIF = 2, //GIF
    JPEG = 3, //JPEG
    PNG = 4, //PNG
    ICON = 5, //ICON
    TIFF = 6, //TIFF
    WMF = 10, //WMF
}

public enum PointMarkerStyle
{
    None = -1, //无图案
    Square = 0, //框
    SquareCheck = 1, //框加勾
    SquareCross = 2, //框加叉
    Circle = 3, //圈
    CirclePoint = 4, //圈加點
    CircleCross = 5, //圈加叉
    Dimond = 6, //钻石
    Triangle = 7, //三角形
    Cross = 8, //叉
    Cross4 = 9, //米字叉
    Star4 = 10, //4角星
    Star5 = 11, //5角星
    Star6 = 12, //6角星
    Star10 = 13, //10角星
}

public enum PrintGenerateStyle
{
    OnlyForm = 1, //只生成報表表單數據。
    OnlyContent = 2, //只生成報表内容數據。
    All = 3, //生成報表所有數據。
    PreviewAll = 4, //预览報表全部内容，但只打印出内容數據。
}

public enum PrintPageType
{
    AllSelectionPages = 1, //输出选定页范围内的所有页。
    OddSelectionPages = 2, //输出选定页范围内的奇數页。
    EvenSelectionPages = 3, //输出选定页范围内的偶數页。
}

public enum PrintRangeType
{
    AllPages = 1, //选定全部页。
    CurrentPage = 2, //选定當前页，只有在从打印御览状態中執行打印任務時才有效。
    SelectionPages = 3, //指定的页范围。
}

public enum PrintType
{
    Form = 1, //表單固定數據，在套打输出内容時不输出。
    Content = 2, //表單内容數據，在套打输出内容時输出。
}

public enum RepeatStyle
{
    None = 1, //明细网格標题不重复输出。
    OnPage = 2, //明细网格標题在每页重复输出。
    OnPageColumn = 4, //明细网格標题在每個页栏重复输出。
    OnGroupHeader = 8, //明细网格標题在最内层的分组頭之后重复输出。
    OnGroupHeaderPage = 10, //明细网格標题在每页與最内层的分组頭之后重复输出。
}

public enum ReportDisplayMode
{
    ScreenView = 1, //正在查詢顯示器中報表處于自画過程中。
    PrintGenerate = 2, //報表處于打印页面生成過程中。
    Idle = 3, //報表不處于任何生成顯示過程中。
}

public enum ScriptType
{
    JScript = 1, //JScript 脚本语言。
    VBScript = 2, //VBScript 脚本语言。
}

public enum SectionType
{
    PageHeader = 1, //页眉。
    ReportHeader = 2, //報表頭。
    ReportFooter = 3, //報表尾。
    PageFooter = 4, //页脚。
    ColumnTitle = 5, //明细网格標题行。
    ColumnContent = 6, //明细网格内容行。
    GroupHeader = 7, //分组頭。
    GroupFooter = 8, //分组尾。
    FreeGridRow = 9, //自由表格行。
}

public enum ShapeType
{
    Circle = 1, //圆。
    Ellipse = 2, //椭圆。
    Rectangle = 3, //矩形。
    RoundRect = 4, //圆角矩形。
    RoundSquare = 5, //圆角正方形。
    Square = 6, //正方形。
    Line = 7, //直線。
}

public enum SharePrintSetupOption
{
    PaperMargin = 1, //页边距项目，對應上、下、左，右页边距屬性。
    PaperKind = 2, //纸张類型项目，對應纸张類型與大小屬性。
    PaperOrientation = 4, //纸张的打印方向项目。
    PaperSource = 8, //纸张的進纸来源项目。
    SelectedPrinter = 16, //當前选定的打印機项目，對應打印機名稱屬性。
}

public enum SheetPages
{
    _1Pages = 1, //指定版面數為1，也就是最常规的單版面输出方式
    _2Pages = 2, //指定版面數為2
    _4Pages = 3, //指定版面數為4
    _6Pages = 4, //指定版面數為6
    _8Pages = 5, //指定版面數為8
    _16Pages = 6, //指定版面數為16
}

public enum ShiftMode
{
    Never = 1, //不進行位移。
    Always = 2, //总是進行位移。
    WhenOverLapped = 3, //只有與上部部件框在垂直方向有重叠時才進行位移。
}

public enum StorageFormat
{
    Text = 1, //文本格式。
    Binary = 2, //二進制格式。
    BinBase64 = 3, //Base64编碼格式，Base64编碼是用可见的ASCII字符表示二進制數據。
}

public enum StringAlignment
{
    Near = 1, //指定文本靠近布局對齐。在左到右布局中，近端位置是左。在右到左布局中，近端位置是右。
    Center = 2, //指定文本在布局矩形中居中對齐。
    Far = 3, //指定文本远離布局矩形的原點位置對齐。在左到右布局中，远端位置是右。在右到左布局中，远端位置是左。
}

public enum SummaryFun
{
    Sum = 1, //统计某個字段的合计值。
    Avg = 2, //统计某個字段的平均值。
    Count = 3, //统计明细記錄的個數。
    Min = 4, //找出某個字段的最小值。
    Max = 5, //找出某個字段的最大值。
    Var = 6, //方差
    VarP = 7, //总體方差
    StdDev = 8, //標准偏差
    StdDevP = 9, //总體標准偏差
    AveDev = 10, //平均偏差
    DevSq = 11, //偏差平方和
    CountBlank = 12, //空值個數
    CountA = 13, //非空值個數
    Distinct = 14, //非重复值個數
    AvgA = 15, //非空值平均
    MinN = 16, //第N個最小值
    MaxN = 17, //第N個最大值
    StrMin = 18, //字符最小值
    StrMax = 19, //字符最大值
    VarA = 20, //非空方差
    VarPA = 21, //非空总體方差
    StdDevA = 22, //非空標准偏差
    StdDevPA = 23, //非空总體標准偏差
    AveDevA = 24, //非空平均偏差
    DevSqA = 25, //非空偏差平方和
    SumAbs = 26, //绝對值合计
    SumAcc = 27, //全程累计，从開始到當前行的合计
    GroupSumAcc = 28, //组累计，在上级组内累计，開始一個新分组，重新累计
    Join = 29, //拼接，將數據顯示文字拼接在一起。各個數據项之间的分隔符號由“格式”值确定。如果要換行，在“格式”加入"\n"字符。如果是统计框，指定
    JoinUnique = 30, //非重复拼接，重复项只拼接一次。其它類同“拼接”函數
}

public enum SystemImage
{
    Radio3DUnchecked = -10, //值為-10，3D形式的无線按钮(Radio)非选中標志。
    Radio3DChecked = -9, //值為-9，3D形式的无線按钮(Radio)选中標志。
    ArrowUp = -8, //值為-8，朝上箭頭標志。
    ArrowDown = -7, //值為-7，朝下箭頭標志。
    Negative = -6, //值為-6，否定標志。
    Affirm = -5, //值為-5，肯定標志。
    _3DUnchecked = -4, //值為-4，3D形式的非选中標志。
    _3DChecked = -3, //值為-3，3D形式的选中標志。
    Unchecked = -2, //值為-2，非选中標志。
    Checked = -1, //值為-1，选中標志。
}

public enum SystemVarType
{
    CurrentDateTime = 1, //计算機的當前日期時间。
    PageCount = 2, //总页數。
    PageNumber = 3, //當前页號。
    RecordNo = 4, //明细記錄的當前記錄號，从1開始计數。
    RowNo = 8, //明细网格的當前行號，从1開始计數。
    RecordCount = 19, //明细記錄的記錄數。
    GroupNo = 20, //分组序號，某個分组的序號，與分组项個數关联，序號从1開始
    GroupCount = 21, //分组數，某個分组產生的分组项個數（全程变量，全程统一值）
    GroupRowNo = 22, //分组项行號，在一個分组内重啟序號，序號从1開始
    GroupRowCount = 23, //分组项行數，某個分组项包含的明细記錄(行)數。
    GroupPageNo = 24, //分组项页號
    GroupPageCount = 25, //分组项页數
}

public enum TextAlign
{
    TopLeft = 17, //内容在垂直方向上顶部對齐，在水平方向上左边對齐。
    TopCenter = 18, //内容在垂直方向上顶部對齐，在水平方向上居中對齐。
    TopRight = 20, //内容在垂直方向上顶部對齐，在水平方向上右边對齐。
    MiddleLeft = 33, //内容在垂直方向上中间對齐，在水平方向上左边對齐。
    MiddleCenter = 34, //内容在垂直方向上中间對齐，在水平方向上居中對齐。
    MiddleRight = 36, //内容在垂直方向上中间對齐，在水平方向上右边對齐。
    BottomLeft = 65, //内容在垂直方向上底边對齐，在水平方向上左边對齐。
    BottomCenter = 66, //内容在垂直方向上底边對齐，在水平方向上居中對齐。
    BottomRight = 68, //内容在垂直方向上底边對齐，在水平方向上右边對齐。
    TopJustiy = 144, //内容在垂直方向上顶部對齐，在水平方向上两端分散對齐。
    MiddleJustiy = 160, //内容在垂直方向上中间對齐，在水平方向上两端分散對齐。
    BottomJustiy = 192, //内容在垂直方向上底边對齐，在水平方向上两端分散對齐。
}

public enum TextEncodeMode
{
    Ansi = 1, //ANSI编碼
    UTF8 = 2, //UTF-8编碼，數據最前面有2個字节的標識數據。
    Unicode = 3, //Unicode编碼
    UTF8Pure = 4, //UTF-8编碼，數據最前面没有標識
    UTF8WithHead = 2, //此项同grtemUTF8，特别注意：枚舉值也應為 2
}

public enum TextOrientation
{
    Default = 0, //默認方式，按从左到右，从上到下方式顯示文字。
    U2DL2R0 = 5, //按从上到下，从左到右方式顯示文字，文字不旋转。
    U2DR2L0 = 9, //按从上到下，从右到左方式顯示文字，文字不旋转。
    D2UL2R1 = 22, //按从下到上，从左到右方式顯示文字，文字旋转。
    U2DR2L1 = 25, //按从上到下，从右到左方式顯示文字，文字旋转。
    L2RD2U0 = 38, //按从左到右，从下到上方式顯示文字，适合用来進行脊背打印，字體應该选择旋转的，即字體名稱前带“@”符號。
    L2RD2U1 = 54, //按从左到右，从下到上方式顯示文字，文字旋转，适合用来進行脊背打印。
    Invert = 58, //倒影方式。
}

public enum Unit
{
    Cm = 1, //以厘米為單位表示報表部件的尺寸與位置。
    Inch = 2, //以英寸為單位表示報表部件的尺寸與位置。
}

public enum ExportImageType
{
    BMP = 1, //Bitmap 位图图像格式。
    PNG = 2, //PNG 图像格式。
    JPEG = 3, //JPEG 图像格式。
    TIFF = 5, //TIFF 图像格式。
}

public enum ExportType
{
    XLS = 1, //导出Excel文件。
    TXT = 2, //导出文本文件。
    HTM = 3, //导出Html超文本文件。
    RTF = 4, //导出RTF文件。
    PDF = 5, //导出PDF格式文件。
    CSV = 6, //导出CSV格式文件。
    IMG = 7, //导出图像文件，支持多种图像格式。
}

public enum PDFDocPermission
{
    Print = 1, //是否允许打印。
    EditAll = 2, //是否允许完整修改文档。
    Copy = 3, //是否允许复制文档内容。
    Edit = 4, //是否允许修改文档。
}

}