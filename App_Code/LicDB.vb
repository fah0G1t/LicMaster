Imports Microsoft.VisualBasic


Namespace LicDB
    Public Class db_struct

        Public Enum lc_tables
            desktop_licenses
            Prodotti
            Clienti
            tipi_licenze_dsk
            licenses_stats
        End Enum

        

        Public Enum tbl_dsktop_lic
            id_req
            id_licenza
            id_cliente
            id_prodotto
            vsn
            c_name
            data_send
            code_send
            dcount
            data_req
            os
            nota
            clnt_app_dir
        End Enum

        Public Enum tbl_dsk_lic_types
            id_tipo
            nome_tipo
        End Enum

        Public Enum tbl_prodotto
            id_prodotto
            nome
            titolo
        End Enum

        Public Enum tbl_clienti
            id_client
            nome_cliente
            telefono
            email
        End Enum

        Public Enum tbl_products_access
            Id
            ProductCode
            ProductVersion
            License
            Ip
            ReferenceCount
            LastAccessDate
        End Enum

        Public Shared Function DBConn() As String
            Return WebCommons.pageCommons.GetConfigValue("dbCon")
        End Function



    End Class
End Namespace


