Imports Paradigm.ORM.Data.Database
Imports Paradigm.ORM.Data.StoredProcedures

Namespace Mocks

    Class SearchZonesStoredProcedure
        Inherits ReaderStoredProcedure(Of SearchZonesParameters, Zone)

        Public Sub New(connector As IDatabaseConnector)
            MyBase.New(connector)
        End Sub

    End Class
End NameSpace