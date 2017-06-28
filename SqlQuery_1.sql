SELECT Technique.Value
FROM RAD_OGP INNER JOIN Technique
ON RAD_OGP.ID_Technique = Technique.ID


SELECT 
	OGP.Name,
	OGP.ID_FPSet,
	RAD_OGP.ID_Technique,
	OGP.ID_kV,
	RAD_OGP.ID_mAs,
	RAD_OGP.ID_ms,
	RAD_OGP.ID_Dose,
	OGP.ID_Focus,
	OGP.ID_FilterType,
	RAD_OGP.ID_ImageAmplification,
	RAD_OGP.ImageAutoamplification,
	RAD_OGP.ID_ImageGradation,
	OGP.ID_ImaSpatialFreqParam,
	RAD_OGP.ImageWinCenter,
	RAD_OGP.ImageWinWidth,
	RAD_OGP.ImageWinAutowindowing,
	OGP.Grid
FROM 
	OGP
		INNER JOIN 
	RAD_OGP
		ON RAD_OGP.ID = OGP.ID
		INNER JOIN
	FPSet
		ON FPSet.ID = OGP.ID_FPSet
		
SELECT 
	OGP.Name,
	FPSet.Name,
	Technique.Value,
	OGP_kV.Value,
	RADOGP_mAs.Value,
	RADOGP_ms.Value,
	Dose_Rad.Dose,
	Focus.Name,
	FilterType.Name,
	ImageAmplification.Value,
	RAD_OGP.ImageAutoamplification,
	GradationParameter.Name,
	SpatialFrequencyParameter.Name,
	RAD_OGP.ImageWinCenter,
	RAD_OGP.ImageWinWidth,
	RAD_OGP.ImageWinAutowindowing,
	RAD_OGP.StandGrid,
	RAD_OGP.StandShutter1,
	RAD_OGP.StandShutter2
FROM (((((((((((OGP 
inner join FPSet ON FPSet.ID = OGP.ID_FPSet)
inner join RAD_OGP ON RAD_OGP.ID = OGP.ID)
inner join Technique ON RAD_OGP.ID_Technique = Technique.ID)
inner join OGP_kV ON OGP.ID_kV = OGP_kV.ID)
inner join RADOGP_mAs ON RAD_OGP.ID_mAs = RADOGP_mAs.ID)
inner join RADOGP_ms ON RAD_OGP.ID_ms = RADOGP_ms.ID)
inner join Dose_Rad ON RAD_OGP.ID_Dose = Dose_Rad.ID)
inner join Focus ON OGP.ID_Focus = Focus.ID)
inner join FilterType ON OGP.ID_FilterType = FilterType.ID)
inner join ImageAmplification ON RAD_OGP.ID_ImageAmplification = ImageAmplification.ID)
inner join GradationParameter ON RAD_OGP.ID_ImageGradation = GradationParameter.IDs)
inner join SpatialFrequencyParameter ON OGP.ID_ImaSpatialFreqParam = SpatialFrequencyParameter.ID



SELECT
T1.Name,
T2.Name
FROM 
OGP T1, DetectorType T2, AcquisitionSystem T3
Where
T1.ID_AcqSystem = T3.ID AND T3.ID_Detector = T2.ID



SELECT
OGP.Name,
FPSet.Name,
Technique.Value,
OGP_kV.Value,
RADOGP_mAs.Value,
RADOGP_ms.Value,
Dose_RAD.Dose,
Focus.Name,
FilterType.Name,
ImageAmplification.Value,
ImageAutoamplification,
GradationParameter.Name,
SpatialFrequencyParameter.Name,
ImageWinCenter,
ImageWinWidth,
ImageWinAutowindowing,
StandShutter1,
StandShutter2
FROM (((((((((((RAD_OGP
left join OGP ON OGP.ID = RAD_OGP.ID)
left join FPSet ON FPSet.ID = OGP.ID_FPSet)
left join Technique ON Technique.ID = RAD_OGP.ID_Technique)
left join OGP_kV ON OGP_kV.ID = OGP.ID_kV)
left join RADOGP_mAs ON RADOGP_mAs.ID = RAD_OGP.ID_mAs)
left join RADOGP_ms ON RADOGP_ms.ID = RAD_OGP.ID_ms)
left join Dose_RAD ON Dose_RAD.ID = RAD_OGP.ID_Dose)
left join Focus ON Focus.ID = OGP.ID_Focus)
left join FilterType ON FilterType.ID = OGP.ID_FilterType)
left join ImageAmplification ON ImageAmplification.ID = RAD_OGP.ID_ImageAmplification)
left join GradationParameter ON GradationParameter.ID = RAD_OGP.ID_ImageGradation)
left join SpatialFrequencyParameter ON OGP.ID_ImaSpatialFreqParam = SpatialFrequencyParameter.ID


SELECT 
	SpatialFrequencyParameter.Name,
	DiamondViewID.Name,
	EdgeFilterKernel.Value,
	SpatialFrequencyParameter.EdgeFilterGain,
	HarmonisKernel.Value,
	SpatialFrequencyParameter.HarmonisGain
FROM ((SpatialFrequencyParameter 
inner join DiamondViewID ON SpatialFrequencyParameter.ID_DiamondViewID = DiamondViewID.ID)
inner join EdgeFilterKernel ON SpatialFrequencyParameter.ID_EdgeFilterKernel = EdgeFilterKernel.ID)
inner join HarmonisKernel ON SpatialFrequencyParameter.ID_HarmonisKernel = HarmonisKernel.ID

	
	
select
  t1.c1
, t2.c2
, t3.c3
, t4.c4
from ((t1
inner join t2 on t1.something = t2.something)
inner join t3 on t2.something = t3.something)
inner join t4 on t3.something = t4.something






SELECT
OGP.Name,
FPSet.Name,
ID_DoseLevel,
OK1.Value,
CharacteristicCurve,
OK2.Value,
Focus.Name,
MaxPulseWidth,
BlackeningCorrection,
O2.Grid,
CollimationSizeX,
CollimationSizeY,
FilterType.Name,
SingleShot,
FixedFrameRate,
AR1.Value,
AR2.Value,
AR3.Value,
WidthFactor,
CenterShift,
Bandwidth,
Center,
Width,
SpatialFrequencyParameter.Name,
kvauto,
Autowindowing
FROM ((((((((((DFR_OGP
left join OGP ON OGP.ID = DFR_OGP.ID)
left join FPSet ON FPSet.ID = OGP.ID_FPSet)
left join OGP_kV AS OK1 ON OK1.ID = OGP.ID_kV)
left join OGP_kV AS OK2 ON OK2.ID = DFR_OGP.ID_DoseReduction)
left join Focus ON Focus.ID = OGP.ID_Focus)
left join OGP AS O2 ON O2.ID = DFR_OGP.ID)
left join FilterType ON FilterType.ID = OGP.ID_FilterType)
left join AcquisitionRate AS AR1 ON AR1.ID = DFR_OGP.ID_AcquisitionRate1)
left join AcquisitionRate AS AR2 ON AR2.ID = DFR_OGP.ID_AcquisitionRate2)
left join AcquisitionRate AS AR3 ON AR3.ID = DFR_OGP.ID_AcquisitionRate3)
left join SpatialFrequencyParameter ON SpatialFrequencyParameter.ID = OGP.ID_ImaSpatialFreqParam




SELECT
OGP.Name,
FPSet.Name,
ID_DoseLevel,
kvauto,
OGP_kV.Value,
CharacteristicCurve,
Focus.Name,
MaxPulseWidth,
BlackeningCorrection,
CollimationSizeY,
CollimationSizeX,
FilterType.Name,
SingleShot,
FixedFrameRate,
AcquisitionRate.Value,
Autowindowing,
WidthFactor,
CenterShift,
Bandwidth,
Center,
Width
SpatialFrequencyParameter.Name,
FROM ((((((DFR_OGP
left join OGP ON OGP.ID = DFR_OGP.ID)
left join FPSet ON FPSet.ID = OGP.ID_FPSet)
left join OGP_kV ON OGP_kV.ID = OGP.ID_kV)
left join Focus ON Focus.ID = OGP.ID_Focus)
left join FilterType ON FilterType.ID = OGP.ID_FilterType)
left join AcquisitionRate ON AcquisitionRate.ID = DFR_OGP.ID_AcquisitionRate1)
left join SpatialFrequencyParameter ON SpatialFrequencyParameter.ID = OGP.ID_ImaSpatialFreqParam





SELECT
AR1.Value,
AR2.Value,
AR3.Value
FROM ((DFR_OGP
left join AcquisitionRate AS AR1 ON AR1.ID = DFR_OGP.ID_AcquisitionRate1)
left join AcquisitionRate AS AR2 ON AR2.ID = DFR_OGP.ID_AcquisitionRate2)
left join AcquisitionRate AS AR3 ON AR3.ID = DFR_OGP.ID_AcquisitionRate3





SELECT
OGP.Name,
FPSet.Name,
ID_DoseLevel,
OK1.Value,
CharacteristicCurve,
OK2.Value,
Focus.Name,
MaxPulseWidth,
BlackeningCorrection,
Grid,
CollimationSizeX,
CollimationSizeY,
FilterType.Name,
SingleShot,
FixedFrameRate,
AR1.Value,
AR2.Value,
AR3.Value,
WidthFactor,
CenterShift,
Bandwidth,
Center,
Width,
SpatialFrequencyParameter.Name,
kvauto,
Autowindowing
FROM((((((((((DFR_OGP
left join OGP ON OGP.ID = DFR_OGP.ID)
left join FPSet ON FPSet.ID = OGP.ID_FPSet)
left join OGP_kV AS OK1 ON OK1.ID = OGP.ID_kV)
left join OGP_kV AS OK2 ON OK2.ID = DFR_OGP.ID_DoseReduction)
left join Focus ON Focus.ID = OGP.ID_Focus)
left join FilterType ON FilterType.ID = OGP.ID_FilterType)
left join AcquisitionRate AS AR1 ON AR1.ID = DFR_OGP.ID_AcquisitionRate1)
left join AcquisitionRate AS AR2 ON AR2.ID = DFR_OGP.ID_AcquisitionRate2)
left join AcquisitionRate AS AR3 ON AR3.ID = DFR_OGP.ID_AcquisitionRate3)
left join SpatialFrequencyParameter ON SpatialFrequencyParameter.ID = OGP.ID_ImaSpatialFreqParam
