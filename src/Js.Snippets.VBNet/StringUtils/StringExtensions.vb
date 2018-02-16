Imports System.Runtime.CompilerServices

Namespace StringUtils

    ''' <summary>
    ''' Extension methods
    ''' </summary>
    <Extension>
    Public Module StringExtensions

        ''' <summary>
        ''' Prefix Of a String With a specified length.
        ''' </summary>
        ''' <param name="value">The String.</param>
        ''' <param name="length">The length Of the prefix.</param>
        ''' <returns>The prefix With the specified length.</returns>
        <Extension>
        Public Function Left(value As String, length As Integer) As String
            If (length < 0) Then
                Throw New ArgumentOutOfRangeException(NameOf(length), "Length cannot be negative.")
            End If

            If (length >= value.Length) Then
                Return value
            End If

            Return value.Substring(0, length)
        End Function

        ''' <summary>
        ''' Suffix Of a string with a specified length.
        ''' </summary>
        ''' <param name="value">The String.</param>
        ''' <param name="length">The length Of the suffix.</param>
        ''' <returns>The suffix With the specified length.</returns>
        <Extension>
        Public Function Right(value As String, length As Integer) As String
            If (length < 0) Then
                Throw New ArgumentOutOfRangeException(NameOf(length), "Length cannot be negative.")
            End If

            If (length >= value.Length) Then
                Return value
            End If

            Return value.Substring(value.Length - length, length)
        End Function

        ''' <summary>
        ''' Determines whether a sequence contains a specified element 
        ''' by using the default equality comparer.
        ''' </summary>
        ''' <param name="value">The value to find.</param>
        ''' <param name="values">A sequence in which to locate a value.</param>
        ''' <returns>True if the sequence contains an element that has the specified value; otherwise, false.</returns>
        <Extension>
        Public Function IsOneOf(value As String, ParamArray values As String()) As Boolean
            Return values.Contains(value)
        End Function

        ''' <summary>
        ''' Converts this string representation Of the name or numeric value of one 
        ''' or more enumerated constants to an equivalent enumerated object.
        ''' </summary>
        ''' <typeparam name = "T">An enumeration type.</typeparam>
        ''' <param name = "value"> The string containing the name or value to convert.</param>
        ''' <param name="ignoreCase">true to ignore case; false to regard case.</param>
        ''' <returns>An object of type <typeparamref name="T"/> whose value is represented by this instance.</returns>
        <Extension>
        Public Function ToEnum(Of T)(value As String, Optional ignoreCase As Boolean = False) As T
            Return CType([Enum].Parse(GetType(T), value, ignoreCase), T)
        End Function

    End Module
End Namespace
