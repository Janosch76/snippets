Namespace FileUtils

    ''' <summary>
    ''' Represents a section of key value-pairs from an INI file.
    ''' </summary>
    Public Class IniFileSection
        Private _sectionName As String
        Private _settings As Dictionary(Of String, String)

        ''' <summary>
        ''' Initializes a new instance of the <see cref="IniFileSection"/> class.
        ''' </summary>
        ''' <param name="SectionName">Name of the section.</param>
        ''' <param name="settings">The settings.</param>
        Public Sub New(SectionName As String, settings As Dictionary(Of String, String))
            _sectionName = SectionName
            _settings = settings
        End Sub

        ''' <summary>
        ''' Gets the name of the section.
        ''' </summary>
        Public ReadOnly Property SectionName As String
            Get
                Return _sectionName
            End Get
        End Property

        ''' <summary>
        ''' Gets the settings.
        ''' </summary>
        Public ReadOnly Property Settings As IReadOnlyDictionary(Of String, String)
            Get
                Return _settings
            End Get
        End Property
    End Class
End Namespace