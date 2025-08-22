Imports System
Imports System.IO
Imports System.IO.Packaging
Imports System.IO.Compression
Imports System.String
Imports Ionic.Zip

Public Class AgroZip


    ''' <summary>
    ''' Method to unzip Zip file at specified location
    ''' </summary>
    Public Shared Sub ZipAFolder(ByVal zipFilePath As String, ByVal CartellaDaComprimere As String)

        Dim zip As New ZipFile
        zip.AddDirectory(CartellaDaComprimere)

        zip.Save(zipFilePath)

    End Sub


    ''' <summary>
    ''' Method to unzip Zip file at specified location
    ''' </summary>
    Public Shared Sub UnZip(ByVal zipFilePath As String, ByVal unZipFolderLocation__1 As String)

        If (zipFilePath <> String.Empty) AndAlso (unZipFolderLocation__1 <> String.Empty) Then

            'vecchia realizzazione...
            'Dim zipFilePackage As System.IO.Packaging.Package = System.IO.Packaging.ZipPackage.Open(zipFilePath, System.IO.FileMode.Open, System.IO.FileAccess.ReadWrite)

            ''Itterate through the all the files that is added within the collection and 
            'For Each contentFile As System.IO.Packaging.ZipPackagePart In zipFilePackage.GetParts()
            '    createFile(unZipFolderLocation__1, contentFile)
            'Next

            'zipFilePackage.Close()
            Dim zip As ZipFile = ZipFile.Read(zipFilePath)
            Dim flagSpecialCharsToEpurate = False
            For Each entry In zip.Entries
                If entry.FileName.Contains(ChrW(15)) = True Then
                    flagSpecialCharsToEpurate = True
                    Exit For
                End If
            Next
            If flagSpecialCharsToEpurate = True Then
                Dim lista = zip.Entries.ToList()
                For Each itm In lista
                    Dim ref = zip.Entries.Where(Function(x) x.FileName = itm.FileName).FirstOrDefault()
                    If ref.FileName.Contains(ChrW(15)) = True Then
                        ref.FileName = ref.FileName.Replace(ChrW(15), "")
                    End If
                Next
            End If

            zip.ExtractAll(unZipFolderLocation__1)

            zip.Dispose()

        End If

    End Sub

    ''' <summary>
    ''' Method to create file at the temp folder
    ''' </summary>
    ''' <param name="rootFolder"></param>
    ''' <param name="contentFileURI"></param>
    ''' <returns></returns>
    Protected Shared Sub createFile(rootFolder As String, contentFile As System.IO.Packaging.ZipPackagePart)
        ' Initially create file under the folder specified
        Dim contentFilePath As String = String.Empty
        contentFilePath = contentFile.Uri.OriginalString.Replace("/"c, System.IO.Path.DirectorySeparatorChar)

        If contentFilePath.StartsWith(System.IO.Path.DirectorySeparatorChar.ToString()) Then
            contentFilePath = contentFilePath.TrimStart(System.IO.Path.DirectorySeparatorChar)
            'do nothing
        Else
        End If

        contentFilePath = System.IO.Path.Combine(rootFolder, contentFilePath)
        'contentFilePath =  System.IO.Path.Combine(rootFolder, contentFilePath); 

        'Check for the folder already exists. If not then create that folder

        If System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(contentFilePath)) <> True Then
            System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(contentFilePath))
            'do nothing
        Else
        End If

        Dim newFileStream As System.IO.FileStream = System.IO.File.Create(contentFilePath)
        newFileStream.Close()
        Dim content As Byte() = New Byte(contentFile.GetStream().Length - 1) {}
        contentFile.GetStream().Read(content, 0, content.Length)
        System.IO.File.WriteAllBytes(contentFilePath, content)

    End Sub


    Private Const BUFFER_SIZE As Long = 4096


    Public Shared Sub AddFileToZip(zipFilename As String, fileToAdd As String, Optional ByVal AbsolutePath As String = "")
        Using zip As Package = System.IO.Packaging.Package.Open(zipFilename, FileMode.OpenOrCreate)

            Dim destFilename As String
            If AbsolutePath = "" Then
                destFilename = ".\" & Path.GetFileName(fileToAdd)
            Else
                destFilename = "." & fileToAdd.Replace(AbsolutePath, "")
            End If


            Dim uri As Uri = PackUriHelper.CreatePartUri(New Uri(destFilename, UriKind.Relative))
            If zip.PartExists(uri) Then
                zip.DeletePart(uri)
            End If

            Dim part As PackagePart = zip.CreatePart(uri, "", CompressionOption.Normal)
            Using fileStream As New FileStream(fileToAdd, FileMode.Open, FileAccess.Read)
                Using dest As Stream = part.GetStream()
                    CopyStream(fileStream, dest)
                End Using
            End Using
        End Using


    End Sub

    Private Shared Sub CopyStream(inputStream As System.IO.FileStream, outputStream As System.IO.Stream)
        Dim bufferSize As Long = If(inputStream.Length < BUFFER_SIZE, inputStream.Length, BUFFER_SIZE)
        Dim buffer As Byte() = New Byte(bufferSize - 1) {}
        Dim bytesRead As Integer = 0
        Dim bytesWritten As Long = 0


        bytesRead = inputStream.Read(buffer, 0, buffer.Length)
        While (bytesRead) <> 0
            outputStream.Write(buffer, 0, bytesRead)
            bytesWritten += bufferSize
            bytesRead = inputStream.Read(buffer, 0, buffer.Length)
        End While

    End Sub

    '  Marco Grilli, 17/06/2014 12:48:51: converte un byte array in una stringa base64
    Private Shared Function ByteArrayToStrBASE64(ByVal byteArray As Byte()) As String
        Return Convert.ToBase64String(byteArray)
    End Function

    '  Marco Grilli, 17/06/2014 12:48:51: converte una stringa base64 in un byte array
    Private Shared Function StrBASE64ToByteArray(ByVal str As String) As Byte()
        Return Convert.FromBase64String(str)
    End Function

#Region "StrToByteArray"

    ' VB.NET to convert a string to a byte array
    Public Shared Function StrToByteArray(ByVal str As String) As Byte()
        'Dim encoding As New System.Text.ASCIIEncoding()
        Dim encoding As New System.Text.UnicodeEncoding()
        Return encoding.GetBytes(str)
    End Function

    ' VB.NET to convert a string to a byte array
    Public Shared Function StrToByteArrayASCII(ByVal str As String) As Byte()
        Dim encoding As New System.Text.ASCIIEncoding()        
        Return encoding.GetBytes(str)
    End Function

    ' VB.NET to convert a string to a byte array
    Public Shared Function StrToByteArrayUTF8(ByVal str As String) As Byte()
        Dim encoding As New System.Text.UTF8Encoding()        
        Return encoding.GetBytes(str)
    End Function
    
    ' VB.NET to convert a string to a byte array
    Public Shared Function StrToByteArrayUTF32(ByVal str As String) As Byte()
        Dim encoding As New System.Text.UTF32Encoding()        
        Return encoding.GetBytes(str)
    End Function

#End Region

#Region "ByteArrayToStr"

    Public Shared Function ByteArrayToStr(ByVal byteArray As Byte(), Optional ByVal removeBOM As Boolean = False) As String        
        Dim myEncoding As New System.Text.UnicodeEncoding()
        Dim stringDati As String = myEncoding.GetString(byteArray)

        If removeBOM Then
            Dim byteOrderMarkUnicode As String = Text.Encoding.Unicode.GetString(Text.Encoding.Unicode.GetPreamble())
            If (stringDati.StartsWith(byteOrderMarkUnicode, StringComparison.Ordinal)) Then
                stringDati = stringDati.Remove(0, byteOrderMarkUnicode.Length)
            End If
        End If

        Return stringDati
    End Function

    Public Shared Function ByteArrayToStrASCII(ByVal byteArray As Byte()) As String
        Dim myEncoding As New System.Text.ASCIIEncoding()        
        Return myEncoding.GetString(byteArray)
    End Function

    Public Shared Function ByteArrayToStrUTF32(ByVal byteArray As Byte(), Optional ByVal removeBOM As Boolean = False) As String
        Dim myEncoding As New System.Text.UTF32Encoding()
        Dim stringDati As String = myEncoding.GetString(byteArray)

        If removeBOM Then
            Dim byteOrderMarkUtf32 As String = Text.Encoding.UTF32.GetString(Text.Encoding.UTF32.GetPreamble())
            If (stringDati.StartsWith(byteOrderMarkUtf32, StringComparison.Ordinal)) Then
                stringDati = stringDati.Remove(0, byteOrderMarkUtf32.Length)
            End If
        End If

        Return stringDati
    End Function

    Public Shared Function ByteArrayToStrUTF8(ByVal byteArray As Byte(), Optional ByVal removeBOM As Boolean = False) As String
        Dim myEncoding As New System.Text.UTF8Encoding()
        Dim stringDati As String = myEncoding.GetString(byteArray)

        If removeBOM Then
            Dim byteOrderMarkUtf8 As String = Text.Encoding.UTF8.GetString(Text.Encoding.UTF8.GetPreamble())
            If (stringDati.StartsWith(byteOrderMarkUtf8, StringComparison.Ordinal)) Then
                stringDati = stringDati.Remove(0, byteOrderMarkUtf8.Length)
            End If
        End If

        Return stringDati
    End Function

#End Region
    
    '################################################################################################
    Public Shared Function Compressione(ByVal ZipMode As Byte, ByVal VettoreByteIn() As Byte) As Byte()

        Dim MemStream As New MemoryStream
        Dim ZipStream As Stream = Nothing

        'Verifico l'algoritmo di compressione richiesto
        Select Case ZipMode

            Case 0  '----- Nessuno --------------------------------
                Return VettoreByteIn

            Case 1  '----- GZip -----------------------------------
                ZipStream = New GZipStream(MemStream, CompressionMode.Compress, True)
                ZipStream.Write(VettoreByteIn, 0, VettoreByteIn.Length)
                ZipStream.Close()
                MemStream.Position = 0
                Dim VettoreByteOut(MemStream.Length - 1) As Byte
                MemStream.Read(VettoreByteOut, 0, MemStream.Length)
                Return VettoreByteOut

            Case 2  '----- Deflate --------------------------------
                ZipStream = New DeflateStream(MemStream, CompressionMode.Compress, True)
                ZipStream.Write(VettoreByteIn, 0, VettoreByteIn.Length)
                ZipStream.Close()
                MemStream.Position = 0
                Dim VettoreByteOut(MemStream.Length - 1) As Byte
                MemStream.Read(VettoreByteOut, 0, MemStream.Length)
                Return VettoreByteOut

            Case Else '--- Non definito ---------------------------
                Return VettoreByteIn

        End Select

    End Function

    ''' <summary>
    ''' Ritorna una stringa codificata in Base64 ma ne prende una tradizionale
    ''' </summary>
    ''' <param name="ZipMode">Modalità compressione: 0 = Nessuna, 1 = GZIP, 2 = Deflate</param>
    ''' <param name="StringIn">Stringa in input tradizionale</param>
    ''' <param name="enc">(Opzionale) Permette di specificare il tipo di stringa arrivata (Unicode, UTF8, ...). Default = Unicode</param>
    Public Shared Function CompressioneBase64(ByVal ZipMode As Byte,
                                              ByVal StringIn As String,
                                              Optional ByVal enc As Text.Encoding = Nothing
                                              ) As String

        '  Marco Grilli, 17/06/2014 12:37:56: converto la stringa in un array di byte
        Dim byteArray As Byte()

        If enc IsNot Nothing AndAlso enc.GetType() = Text.Encoding.UTF8.GetType() Then
            byteArray = StrToByteArrayUTF8(StringIn)
        ElseIf enc IsNot Nothing AndAlso enc.GetType() = Text.Encoding.Unicode.GetType() Then
            byteArray = StrToByteArray(StringIn)
        ElseIf enc IsNot Nothing AndAlso enc.GetType() = Text.Encoding.UTF32.GetType() Then
            byteArray = StrToByteArrayUTF32(StringIn)
        ElseIf enc IsNot Nothing AndAlso enc.GetType() = Text.Encoding.ASCII.GetType() Then
            byteArray = StrToByteArrayASCII(StringIn)
        Else
            byteArray = StrToByteArray(StringIn)
        End If

        '  Marco Grilli, 17/06/2014 12:37:56: comprimo come al solito
        Dim byteOut As Byte() = Compressione(ZipMode, byteArray)

        '  Marco Grilli, 17/06/2014 12:40:31: ritorno l'array convertito in stringa Base64
        Return ByteArrayToStrBASE64(byteOut)

    End Function

    Public Shared Function CompressioneBase64PerJS(ByVal ZipMode As Byte,
                                              ByVal StringIn As String,
                                              Optional ByVal enc As Text.Encoding = Nothing
                                              ) As Byte()

        '  Marco Grilli, 17/06/2014 12:37:56: converto la stringa in un array di byte
        Dim byteArray As Byte()

        If enc IsNot Nothing AndAlso enc.GetType() = Text.Encoding.UTF8.GetType() Then
            byteArray = StrToByteArrayUTF8(StringIn)
        ElseIf enc IsNot Nothing AndAlso enc.GetType() = Text.Encoding.Unicode.GetType() Then
            byteArray = StrToByteArray(StringIn)
        ElseIf enc IsNot Nothing AndAlso enc.GetType() = Text.Encoding.UTF32.GetType() Then
            byteArray = StrToByteArrayUTF32(StringIn)
        ElseIf enc IsNot Nothing AndAlso enc.GetType() = Text.Encoding.ASCII.GetType() Then
            byteArray = StrToByteArrayASCII(StringIn)
        Else
            byteArray = StrToByteArray(StringIn)
        End If

        '  Marco Grilli, 17/06/2014 12:37:56: comprimo come al solito
        Dim byteOut As Byte() = Compressione(ZipMode, byteArray)

        '  Marco Grilli, 17/06/2014 12:40:31: ritorno l'array convertito in stringa Base64
        Return byteOut

    End Function

    ''' <summary>
    ''' Ritorna una stringa tradizionale, ma ne prende una codificata in Base64
    ''' </summary>
    ''' <param name="ZipMode">Modalità compressione: 0 = Nessuna, 1 = GZIP, 2 = Deflate</param>
    ''' <param name="arrayByte">arrayByte con contenuto compresso
    ''' <param name="enc">(Opzionale) Permette di specificare il tipo di stringa arrivata (Unicode, UTF8, ...). Default = Unicode</param>
    ''' <param name="removeBOM">(Opzionale) Flag per forzare rimozione di Byte Order Mark se presente, per avere in output stringa pulita di questi byte iniziali</param>
    Public Shared Function DeCompressioneBase64PerJs(ByVal ZipMode As Byte,
                                                ByVal arrayByte As Byte(),
                                                Optional ByVal enc As Text.Encoding = Nothing,
                                                Optional ByVal removeBOM As Boolean = False
                                                ) As String

        '  Marco Grilli, 17/06/2014 12:37:56: decomprimo come al solito
        Dim byteOut As Byte() = DeCompressione(ZipMode, arrayByte)

        If enc IsNot Nothing AndAlso enc.GetType() = Text.Encoding.UTF8.GetType() Then
            Return ByteArrayToStrUTF8(byteOut, removeBOM)
        ElseIf enc IsNot Nothing AndAlso enc.GetType() = Text.Encoding.Unicode.GetType() Then
            Return ByteArrayToStr(byteOut, removeBOM)
        ElseIf enc IsNot Nothing AndAlso enc.GetType() = Text.Encoding.UTF32.GetType() Then
            Return ByteArrayToStrUTF32(byteOut, removeBOM)
        ElseIf enc IsNot Nothing AndAlso enc.GetType() = Text.Encoding.ASCII.GetType() Then
            Return ByteArrayToStrASCII(byteOut)
        Else
            '  Marco Grilli, 17/06/2014 12:40:31: ritorno l'array convertito in stringa tradizionale
            Return ByteArrayToStr(byteOut)
        End If

    End Function


    '################################################################################################
    Public Shared Function DeCompressione(ByVal ZipMode As Byte, ByVal VettoreByteIn() As Byte) As Byte()

        Dim MemStream As New MemoryStream(VettoreByteIn)
        Dim ZipStream As Stream = Nothing

        'Verifico l'algoritmo di compressione richiesto
        Select Case ZipMode

            Case 0  '----- Nessuno --------------------------------
                Return VettoreByteIn

            Case 1  '----- GZip -----------------------------------
                ZipStream = New GZipStream(MemStream, CompressionMode.Decompress, True)
                Dim VettoreByteOut() As Byte
                VettoreByteOut = RetrieveBytesFromStream(ZipStream, VettoreByteIn.Length)
                Return VettoreByteOut

            Case 2  '----- Deflate --------------------------------
                ZipStream = New DeflateStream(MemStream, CompressionMode.Decompress, True)
                Dim VettoreByteOut() As Byte
                VettoreByteOut = RetrieveBytesFromStream(ZipStream, VettoreByteIn.Length)
                Return VettoreByteOut

            Case Else '--- Non definito ---------------------------
                Return VettoreByteIn

        End Select

    End Function

    ''' <summary>
    ''' Ritorna una stringa tradizionale, ma ne prende una codificata in Base64
    ''' </summary>
    ''' <param name="ZipMode">Modalità compressione: 0 = Nessuna, 1 = GZIP, 2 = Deflate</param>
    ''' <param name="StringIn">Stringa in input in Base64</param>
    ''' <param name="enc">(Opzionale) Permette di specificare il tipo di stringa arrivata (Unicode, UTF8, ...). Default = Unicode</param>
    ''' <param name="removeBOM">(Opzionale) Flag per forzare rimozione di Byte Order Mark se presente, per avere in output stringa pulita di questi byte iniziali</param>
    Public Shared Function DeCompressioneBase64(ByVal ZipMode As Byte,
                                                ByVal StringIn As String,
                                                Optional ByVal enc As Text.Encoding = Nothing,
                                                Optional ByVal removeBOM As Boolean = False
                                                ) As String

        '  Marco Grilli, 17/06/2014 12:37:56: converto la stringa base64 in un array di byte
        Dim byteArray As Byte() = StrBASE64ToByteArray(StringIn)
        '  Marco Grilli, 17/06/2014 12:37:56: decomprimo come al solito
        Dim byteOut As Byte() = DeCompressione(ZipMode, byteArray)

        If enc IsNot Nothing AndAlso enc.GetType() = Text.Encoding.UTF8.GetType() Then
            Return ByteArrayToStrUTF8(byteOut, removeBOM)
        ElseIf enc IsNot Nothing AndAlso enc.GetType() = Text.Encoding.Unicode.GetType() Then
            Return ByteArrayToStr(byteOut, removeBOM)
        ElseIf enc IsNot Nothing AndAlso enc.GetType() = Text.Encoding.UTF32.GetType() Then
            Return ByteArrayToStrUTF32(byteOut, removeBOM)
        ElseIf enc IsNot Nothing AndAlso enc.GetType() = Text.Encoding.ASCII.GetType() Then
            Return ByteArrayToStrASCII(byteOut)
        Else
            '  Marco Grilli, 17/06/2014 12:40:31: ritorno l'array convertito in stringa tradizionale
            Return ByteArrayToStr(byteOut)
        End If

    End Function

    Public Shared Function DeCompressioneBase64Byte(ByVal ZipMode As Byte, ByVal StringIn As String) As Byte()

        '  Marco Grilli, 17/06/2014 12:37:56: converto la stringa base64 in un array di byte
        Dim byteArray As Byte() = StrBASE64ToByteArray(StringIn)
        '  Marco Grilli, 17/06/2014 12:37:56: decomprimo come al solito
        Dim byteOut As Byte() = DeCompressione(ZipMode, byteArray)

        '  Marco Grilli, 17/06/2014 12:40:31: ritorno l'array di byte
        Return byteOut

    End Function

    '################################################################################################
    ''' <summary>
    ''' ---retrieve the bytes from a stream object---
    ''' </summary>
    Public Shared Function RetrieveBytesFromStream(ByVal stream As Stream, ByVal bytesblock As Integer) As Byte()

        Const nomeRoutine = "AgroZip.RetrieveBytesFromStream"
        Dim data() As Byte
        Dim totalCount As Integer = 0
        Try
            While True
                '---progressively increase the size of the data byte array---
                ReDim Preserve data(totalCount + bytesblock)
                Dim bytesRead As Integer = stream.Read(data, totalCount, bytesblock)
                If bytesRead = 0 Then
                    Exit While
                End If
                totalCount += bytesRead
            End While
            '---make sure the byte array contains exactly the number 
            ' of bytes extracted---
            ReDim Preserve data(totalCount - 1)
            Return data
        Catch ex As Exception
            Throw New Exception("[" & nomeRoutine & "] : " & ex.Message)
        End Try

    End Function

    '###############################################################################################
    '###############################################################################################
    '###############################################################################################
    '###############################################################################################
    '###############################################################################################


End Class
