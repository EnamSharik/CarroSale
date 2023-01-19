SELECT VEHICLE.PLACA AS PLACA, OWNER.NAME1 + ' ' +OWNER.LASTN1 AS NAME, TRADEMARK.NAME +' '+ MODEL.NAME AS DESCRIPTION, VEHICLE.YEAR AS YEAR, VEHICLE.STATE AS STATE
FROM VEHICLE
INNER JOIN OWNER ON VEHICLE.ID_OWNER = OWNER.ID_OWNER
INNER JOIN MODEL ON VEHICLE.ID_MODEL = MODEL.ID_MODEL
INNER JOIN TRADEMARK ON MODEL.ID_TRADEMARK = TRADEMARK.ID_TRADEMARK 