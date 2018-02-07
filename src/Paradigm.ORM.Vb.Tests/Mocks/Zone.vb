Imports Paradigm.ORM.Data.Attributes

Namespace Mocks

    <Table("zone")>
    Class Zone

        <Column>
        <PrimaryKey>
        <Identity>
        Public Property Id As Integer

        <Column>
        Public Property ParentZoneId as Integer?

        <Column>
        Public Property CompanyId as Integer

        <Column>
        Public Property SubZoneId as Integer

        <Column>
        Public Property StatusId as Integer

        <Column>
        Public Property Code as String

        <Column>
        Public Property Name As String

        <Column>
        Public Property Description As String

        <Column>
        Public Property Latitude As Decimal?

        <Column>
        Public Property Longitude As Decimal?

    End Class
End NameSpace