Namespace FileUtils

    Public Class IniFile
        Implements IEnumerable(Of IniFileSection)

        Private sections As Dictionary(Of String, IniFileSection)

        Private Sub New()
            Me.sections = New Dictionary(Of String, IniFileSection)
        End Sub

        Public ReadOnly Property SectionNames As IEnumerable(Of String)
            Get
                Return Me.sections.Keys
            End Get
        End Property

        Public Shared Function Read(filename As String) As IniFile
            Using reader = New IniReader(filename)
                Return Read(reader)
            End Using
        End Function

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

                    If iniFile.sections.ContainsKey(sectionName) Then
                        Throw New Exception($"Error: duplicate section name {sectionName}")
                    End If

                    Dim section = New IniFileSection(sectionName, settings)
                    iniFile.sections.Add(sectionName, section)
                Else
                    Throw New FormatException($"Error: Line format not recognized [{reader.CurrentRow}]")
                End If
            Loop

            Return iniFile
        End Function

        Public Iterator Function GetEnumerator() As IEnumerator(Of IniFileSection) Implements IEnumerable(Of IniFileSection).GetEnumerator
            For Each section In Me.sections
                Yield section.Value
            Next
        End Function

        Private Function IEnumerable_GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
            Return GetEnumerator()
        End Function
    End Class
End Namespace
