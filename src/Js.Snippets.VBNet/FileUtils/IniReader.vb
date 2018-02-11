
Imports System.IO
Imports System.Text

Namespace FileUtils

    ''' <summary>
    ''' A reader for parsing the lines from an INI file.
    ''' </summary>
    ''' <seealso cref="System.IDisposable" />
    Public NotInheritable Class IniReader
        Implements IDisposable

        Private ReadOnly _reader As TextReader
        Private _disposedValue As Boolean ' To detect redundant calls
        Private _bufferedLine As String ' invariant: bufferedLine Is Nothing only if reader.EndOfStream
        Private _currentRowValue As Integer

        ''' <summary>
        ''' Initializes a new instance of the <see cref="IniReader"/> class.
        ''' </summary>
        ''' <param name="path">The filename.</param>
        Public Sub New(ByVal path As String)
            Me.New(New FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
        End Sub

        ''' <summary>
        ''' Initializes a new instance of the <see cref="IniReader"/> class.
        ''' </summary>
        ''' <param name="stream">The stream.</param>
        Public Sub New(ByVal stream As Stream)
            Me.New(New StreamReader(stream, Encoding.UTF8))
        End Sub

        ''' <summary>
        ''' Initializes a new instance of the <see cref="IniReader"/> class.
        ''' </summary>
        ''' <param name="reader">The stream reader.</param>
        Public Sub New(ByVal reader As TextReader)
            _reader = reader
            _bufferedLine = reader.ReadLine
            _currentRowValue = 0
        End Sub

        ''' <summary>
        ''' Gets a value indicating whether the end of the input stream has been reached.
        ''' </summary>
        Public ReadOnly Property EndOfStream As Boolean
            Get
                Return _bufferedLine Is Nothing
            End Get
        End Property

        ''' <summary>
        ''' Gets the current row number.
        ''' </summary>
        Public ReadOnly Property CurrentRow As Integer
            Get
                Return _currentRowValue
            End Get
        End Property

        ''' <summary>
        ''' Tries to parse the current line as a comment.
        ''' </summary>
        ''' <returns>True, if the current line was recognized as a comment.</returns>
        Public Function TryParseComment() As Boolean
            If _bufferedLine IsNot Nothing AndAlso IsComment(_bufferedLine) Then
                _bufferedLine = _reader.ReadLine
                Return True
            Else
                Return False
            End If
        End Function

        ''' <summary>
        ''' Tries the parse the current line as section heading.
        ''' </summary>
        ''' <param name="sectionName">Out parameter: Name of the section.</param>
        ''' <returns>True, if the current line was recognized as a section heading.</returns>
        Public Function TryParseSectionHeading(ByRef sectionName As String) As Boolean
            If _bufferedLine IsNot Nothing AndAlso isSectionHeading(_bufferedLine) Then
                sectionName = _bufferedLine.Replace("[", "").Replace("]", "").Trim()
                _bufferedLine = _reader.ReadLine
                Return True
            Else
                Return False
            End If
        End Function

        ''' <summary>
        ''' Tries to parse the current line as section entry.
        ''' </summary>
        ''' <param name="entry">Out parameter: The entry.</param>
        ''' <returns>True, if the current line was recognized as a section entry.</returns>
        Public Function TryParseSectionEntry(ByRef entry As KeyValuePair(Of String, String)) As Boolean
            If _bufferedLine IsNot Nothing AndAlso IsSectionEntry(_bufferedLine) Then
                Dim keyValueArray = _bufferedLine.Split("="c)
                entry = New KeyValuePair(Of String, String)(keyValueArray(0).Trim(), keyValueArray(1).Trim())
                _bufferedLine = _reader.ReadLine
                Return True
            Else
                Return False
            End If
        End Function

        ''' <summary>
        ''' Closes the underlying stream.
        ''' </summary>
        Public Sub Dispose() Implements IDisposable.Dispose
            If Not _disposedValue Then
                If _reader IsNot Nothing Then
                    _reader.Dispose()
                End If
            End If

            _disposedValue = True
        End Sub

        Private Function ReadLine() As String
            Dim bufferedLine As String = _bufferedLine
            _bufferedLine = _reader.ReadLine
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