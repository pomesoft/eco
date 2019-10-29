

select * from ContactosHP


delete from Parametros where Descripcion = 'REMITENTE_AVISOS'
insert into Parametros (Descripcion, Valor, Vigente) values ('REMITENTE_AVISOS', 'notificaciones@eco-pms.com.ar', 1)

delete from Parametros where Descripcion = 'SMTP_HOST'
insert into Parametros (Descripcion, Valor, Vigente) values ('SMTP_HOST', 'mail.eco-pms.com.ar', 1)

delete from Parametros where Descripcion = 'SMTP_PUERTO'
insert into Parametros (Descripcion, Valor, Vigente) values ('SMTP_PUERTO', '587', 1)

delete from Parametros where Descripcion = 'SMTP_USER'
insert into Parametros (Descripcion, Valor, Vigente) values ('SMTP_USER', 'notificaciones@eco-pms.com.ar', 1)

delete from Parametros where Descripcion = 'SMTP_PWD'
insert into Parametros (Descripcion, Valor, Vigente) values ('SMTP_PWD', 'Eco1234+', 1)

delete from Parametros where Descripcion = 'EMAIL_CONTACTOHOMEPAGE_ASUNTO'
insert into Parametros (Descripcion, Valor, Vigente) values ('EMAIL_CONTACTOHOMEPAGE_ASUNTO', 'ECO - Contacto desde Home Page', 1)

delete from Parametros where Descripcion = 'EMAIL_CONTACTOHOMEPAGE_DESTINATARIOS'
insert into Parametros (Descripcion, Valor, Vigente) values ('EMAIL_CONTACTOHOMEPAGE_DESTINATARIOS', 'info@eco-pms.com.ar;ajmontigel@gmail.com', 1)

select * from Parametros
