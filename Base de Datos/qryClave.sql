
/*Comité de Etica en Famacología Clínica de la Fundación CIDEA03-2013pome@soft

key = "QEzBx86jTAs=";
claveEncriptada = "/t7r1s9h9AqsgAR3bHOvFMMJzxyY+b2IATXgAwLK6emAnhl4npjqeKHb4Ui/7i3rTdnlYFIDQRzMBwTZqjERNzALHHQUdrn/fkF/Ydy+BUvxcuoI0IobY2FgnkaO8ifmBbugZ2qn574ZRMZn+dkGmyJLRKZvUpWP+gho+Bq2PSuqXk+6A+mwq7jt7VZjbueRLY0WWnU/38L/v4D7kaUB3g==";
*/



/*CIDEA
IdParametro Descripcion                                                                                          Valor                                                                                                                                                                                                                                                      Vigente
----------- ---------------------------------------------------------------------------------------------------- ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- -------
3           Comite                                                                                               Comité de Etica en Famacología Clínica de la Fundación CIDEA                                                                                                                                                                                               1
4           FechaImplementacion                                                                                  03-2013                                                                                                                                                                                                                                                    1


INSERT INTO Parametros (Descripcion, Valor, Vigente)
VALUES ('Comite', 'Comité de Etica en Famacología Clínica de la Fundación CIDEA', 1)
INSERT INTO Parametros (Descripcion, Valor, Vigente)
VALUES ('FechaImplementacion', '03-2013', 1)
*/

/*CEYFYC

DELETE FROM Parametros WHERE IdParametro IN (18)
INSERT [dbo].[Parametros] ([Descripcion], [Valor], [Vigente]) VALUES ( N'Comite', N'CEIFyC Fundacion CIDEA', 1)
INSERT [dbo].[Parametros] ([Descripcion], [Valor], [Vigente]) VALUES ( N'FechaImplementacion', N'05-2013', 1)
INSERT [dbo].[Parametros] ([Descripcion], [Valor], [Vigente]) VALUES ( N'URL_SITE', N'http://localhost:1212', 1)

*/

select * from Parametros

select * from Usuarios

select * from Actas