Imports System.Globalization
Imports System.Runtime.CompilerServices

Namespace DateTimeUtils

    ''' <summary>
    ''' Extension methods
    ''' </summary>
    <Extension>
    Public Module DateTimeExtensions

        ''' <summary>
        ''' Formats the date in ISO format.
        ''' </summary>
        ''' <param name="date">The date.</param>
        ''' <returns>The ISO format string representation of the date.</returns>
        <Extension>
        Public Function ToDateIso([date] As DateTime) As String
            Return [date].ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)
        End Function

        ''' <summary>
        ''' Formats the timestamp in ISO format.
        ''' </summary>
        ''' <param name="timestamp">The timestamp.</param>
        ''' <returns>The ISO format string representation of the timestamp.</returns>
        <Extension>
        Public Function ToDateTimeIso(timestamp As DateTime) As String
            Return timestamp.ToString("o")
        End Function

    End Module

End Namespace
