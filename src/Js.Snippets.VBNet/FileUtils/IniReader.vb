
Imports System.IO
Imports System.Text

Namespace FileUtils

    Public Class IniReader
        Implements IDisposable

        Private ReadOnly reader As TextReader
        Private disposedValue As Boolean ' To detect redundant calls
        Private bufferedLine As String ' invariant: bufferedLine Is Nothing only if reader.EndOfStream
        Private currentRowValue As Integer

        Public Sub New(ByVal path As String)
            Me.New(New FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
        End Sub

        Public Sub New(ByVal stream As Stream)
            Me.New(New StreamReader(stream, Encoding.UTF8))
        End Sub

        Public Sub New(ByVal reader As TextReader)
            Me.reader = reader
            Me.bufferedLine = reader.ReadLine
            Me.currentRowValue = 0
        End Sub

        Public ReadOnly Property EndOfStream As Boolean
            Get
                Return Me.bufferedLine Is Nothing
            End Get
        End Property

        Public ReadOnly Property CurrentRow As Integer
            Get
                Return Me.currentRowValue
            End Get
        End Property

        Public Function TryParseComment() As Boolean
            If bufferedLine IsNot Nothing AndAlso IsComment(bufferedLine) Then
                bufferedLine = reader.ReadLine
                Return True
            Else
                Return False
            End If
        End Function

        Public Function TryParseSectionHeading(ByRef sectionName As String) As Boolean
            If bufferedLine IsNot Nothing AndAlso isSectionHeading(bufferedLine) Then
                sectionName = bufferedLine.Replace("[", "").Replace("]", "").Trim
                bufferedLine = reader.ReadLine
                Return True
            Else
                Return False
            End If
        End Function

        Public Function TryParseSectionEntry(ByRef entry As KeyValuePair(Of String, String)) As Boolean
            If bufferedLine IsNot Nothing AndAlso IsSectionEntry(bufferedLine) Then
                Dim keyValueArray = bufferedLine.Split("="c)
                entry = New KeyValuePair(Of String, String)(keyValueArray(0).Trim, keyValueArray(1).Trim)
                bufferedLine = reader.ReadLine
                Return True
            Else
                Return False
            End If
        End Function

        Public Sub Dispose() Implements IDisposable.Dispose
            Dispose(True)
        End Sub

        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not disposedValue Then
                If disposing Then
                    reader.Dispose()
                End If
            End If
            disposedValue = True
        End Sub

        Private Function ReadLine() As String
            Dim bufferedLine As String = Me.bufferedLine
            Me.bufferedLine = Me.reader.ReadLine
            Return bufferedLine
        End Function

        Private Function IsComment(ByVal line As String) As Boolean
            If (String.IsNullOrEmpty(line.Trim())) Then
                Return True
            End If

            If (line.Trim().StartsWith(";")) Then
                Return True
            End If

            Return False
        End Function

        Private Function isSectionHeading(ByVal line As String) As Boolean
            If (line.Trim().StartsWith("[") AndAlso line.Trim().EndsWith("]")) Then
                Return True
            End If

            Return False
        End Function

        Private Function IsSectionEntry(ByVal line As String) As Boolean
            Dim entry As String() = line.Split("="c)
            Return entry.Length = 2 _
            AndAlso Not String.IsNullOrEmpty(entry(0)) _
            AndAlso Not String.IsNullOrEmpty(entry(1))
        End Function

    End Class
End Namespace