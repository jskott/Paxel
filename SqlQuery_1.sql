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
	OGP.Grid
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


	
	
select
  t1.c1
, t2.c2
, t3.c3
, t4.c4
from ((t1
inner join t2 on t1.something = t2.something)
inner join t3 on t2.something = t3.something)
inner join t4 on t3.something = t4.something