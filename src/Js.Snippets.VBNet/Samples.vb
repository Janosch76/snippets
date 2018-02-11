Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports System.IO
Imports Js.Snippets.VBNet.StringUtils
Imports Js.Snippets.VBNet.FileUtils

<TestClass()>
Public Class Samples

    <TestMethod()>
    Public Sub Left()
        Assert.AreEqual("ab", "abc".Left(2))
        Assert.AreEqual("", "abc".Left(0))
        Assert.AreEqual("abc", "abc".Left(5))
        Assert.AreEqual("", "".Left(5))
    End Sub

    <TestMethod()>
    Public Sub Right()
        Assert.AreEqual("bc", "abc".Right(2))
        Assert.AreEqual("", "abc".Right(0))
        Assert.AreEqual("abc", "abc".Right(5))
        Assert.AreEqual("", "".Right(5))
    End Sub

    <TestMethod()>
    Public Sub IsOneOf()
        Assert.AreEqual(True, "b".IsOneOf("a", "b", "c"))
        Assert.AreEqual(False, "B".IsOneOf("a", "b", "c"))
        Assert.AreEqual(False, "d".IsOneOf("a", "b", "c"))
    End Sub

    <TestMethod()>
    Public Sub ToEnum()
        Assert.AreEqual(TaskStatus.Canceled, "Canceled".ToEnum(Of TaskStatus)())
        Assert.AreEqual(TaskStatus.Canceled, "CANCELED".ToEnum(Of TaskStatus)(ignoreCase:=True))
    End Sub

    <TestMethod()>
    Public Sub IniReader()
        Dim ini As String = "
            [Section 1]
            key1=0
            key2=C:\Temp

            [Section 2]
            key1=abc"

        Dim iniFile As IniFile = IniFile.Read(New IniReader(New StringReader(ini)))

        Assert.AreEqual(2, iniFile.SectionNames.Count())
        Assert.AreEqual(True, iniFile.SectionNames.Contains("Section 1"))
        Assert.AreEqual(True, iniFile.SectionNames.Contains("Section 2"))

        Assert.AreEqual("0", iniFile.Sections("Section 1").Settings("key1"))
        Assert.AreEqual("C:\Temp", iniFile.Sections("Section 1").Settings("key2"))
    End Sub
End Class