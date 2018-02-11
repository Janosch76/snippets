Namespace FileUtils

    Public Class IniFileSection
        Private sectionNameValue As String
        Private settingsValue As Dictionary(Of String, String)

        Public Sub New(SectionName As String, settings As Dictionary(Of String, String))
            Me.sectionNameValue = SectionName
            Me.settingsValue = settings
        End Sub

        Public ReadOnly Property SectionName As String
            Get
                Return Me.sectionNameValue
            End Get
        End Property

        Public ReadOnly Property Settings As IReadOnlyDictionary(Of String, String)
            Get
                Return Me.settingsValue
            End Get
        End Property
    End Class
End Namespace