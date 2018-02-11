Namespace FileUtils

    ''' <summary>
    ''' Represents an INI file with settings grouped into sections
    ''' </summary>
    Public Class IniFile
        Implements IEnumerable(Of IniFileSection)

        Private _sections As Dictionary(Of String, IniFileSection)

        ''' <summary>
        ''' Prevents a default instance of the <see cref="IniFile"/> class from being created.
        ''' </summary>
        Private Sub New()
            _sections = New Dictionary(Of String, IniFileSection)
        End Sub

        ''' <summary>
        ''' Gets the section names.
        ''' </summary>
        Public ReadOnly Property SectionNames() As IEnumerable(Of String)
            Get
                Return _sections.Keys
            End Get
        End Property

        ''' <summary>
        ''' Gets the section with a specified section name.
        ''' </summary>
        Public ReadOnly Property Sections(sectionName As String) As IniFileSection
            Get
                Return _sections(sectionName)
            End Get
        End Property

        ''' <summary>
        ''' Reads the INI file with the specified filename.
        ''' </summary>
        ''' <param name="filename">The filename.</param>
        ''' <returns>The settings parsed from the INI file with the given filename.</returns>
        Public Shared Function Read(filename As String) As IniFile
            Using reader = New IniReader(filename)
                Return Read(reader)
            End Using
        End Function

        ''' <summary>
        ''' Reads the INI file using the specified reader.
        ''' </summary>
        ''' <param name="reader">The reader.</param>
        ''' <returns>The settings parsed from the INI file with the given reader.</returns>
        Public Shared Function Read(reader As IniReader) As IniFile
            Dim iniFile = New IniFile()

            Do Until reader.EndOfStream
                Dim sectionName As String = Nothing
                If reader.TryParseComment() Then
                    ' nothing to do (ignore comments)
                ElseIf reader.TryParseSectionHeading(sectionName) Then
                    ' parse section entries
                    Dim settings As New Dictionary(Of String, String)
                    Dim entry As KeyValuePair(Of String, String) = Nothing
                    While reader.TryParseSectionEntry(entry)
                        If settings.ContainsKey(entry.Key) Then
                            Throw New Exception($"Error: duplicate entry {entry.Key} in section {sectionName}")
                        End If

                        settings.Add(entry.Key, entry.Value)
                    End While

                    If iniFile._sections.ContainsKey(sectionName) Then
                        Throw New Exception($"Error: duplicate section name {sectionName}")
                    End If

                    Dim section = New IniFileSection(sectionName, settings)
                    iniFile._sections.Add(sectionName, section)
                Else
                    Throw New FormatException($"Error: Line format not recognized [{reader.CurrentRow}]")
                End If
            Loop

            Return iniFile
        End Function

        ''' <summary>
        ''' Returns an enumerator that iterates through the collection.
        ''' </summary>
        ''' <returns>
        ''' A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.
        ''' </returns>
        Public Iterator Function GetEnumerator() As IEnumerator(Of IniFileSection) Implements IEnumerable(Of IniFileSection).GetEnumerator
            For Each sect In _sections
                Yield sect.Value
            Next
        End Function

        Private Function IEnumerable_GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
            Return GetEnumerator()
        End Function
    End Class
End Namespace
