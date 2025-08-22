Imports System.Runtime.Serialization
Imports System.Security

<Serializable()>
Public Class GiasException
    Inherits System.Exception
    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal message As String)
        MyBase.New(message)
    End Sub

    Public Sub New(ByVal message As String, ByVal innerException As System.Exception)
        MyBase.New(message, innerException)
    End Sub

    ' Constructor required for serialization
    <SecuritySafeCritical()>
    Protected Sub New(ByVal info As SerializationInfo, ByVal context As StreamingContext)
        MyBase.New(info, context)
    End Sub
End Class

<Serializable()>
Public Class RecoverableSPIDMultipleAccountLoginException
    Inherits System.Exception
    Public Risposta As UserLinkedAccountRisposta

    Public Sub New(risp As UserLinkedAccountRisposta)
        Risposta = risp
    End Sub
End Class
''' <summary>
''' Eccezione di tipo login
''' </summary>
<Serializable()>
Public Class AgroEccezioni_LoginFallito_Exception
    Inherits GiasException

    ''' <summary>
    ''' Initializes a new instance of the <see cref="AgroEccezioni_LoginFallito_Exception"/> class.
    ''' </summary>
    Public Sub New()
        MyBase.New()
    End Sub

    ''' <summary>
    ''' Initializes a new instance of the <see cref="AgroEccezioni_LoginFallito_Exception"/> class
    ''' with the specified error message.
    ''' </summary>
    ''' <param name="message">The message that describes the error.</param>
    Public Sub New(ByVal message As String)
        MyBase.New(message)
    End Sub

    ''' <summary>
    ''' Initializes a new instance of the <see cref="AgroEccezioni_LoginFallito_Exception"/> class
    ''' with the specified error message and a reference to the inner
    ''' exception that is the cause of this exception.
    ''' </summary>
    ''' <param name="message">The message that describes the error.</param>
    ''' <param name="innerException">The exception that is the cause of the
    ''' current exception, or a null reference if no inner exception is
    ''' specified</param>
    Public Sub New(ByVal message As String, ByVal innerException As System.Exception)
        MyBase.New(message, innerException)
    End Sub

    ' Constructor required for serialization
    <SecuritySafeCritical()>
    Protected Sub New(ByVal info As SerializationInfo, ByVal context As StreamingContext)
        MyBase.New(info, context)
    End Sub

End Class

''' <summary>
''' Eccezione di tipo particella con legami appezzamenti/campi
''' </summary>
<Serializable()>
Public Class AgroEccezioni_ParticellaConLegami_Exception
    Inherits GiasException

    ''' <summary>
    ''' Initializes a new instance of the <see cref="AgroEccezioni_ParticellaConLegami_Exception"/> class.
    ''' </summary>
    Public Sub New()
        MyBase.New()
    End Sub

    ''' <summary>
    ''' Initializes a new instance of the <see cref="AgroEccezioni_ParticellaConLegami_Exception"/> class
    ''' with the specified error message.
    ''' </summary>
    ''' <param name="message">The message that describes the error.</param>
    Public Sub New(ByVal message As String)
        MyBase.New(message)
    End Sub

    ''' <summary>
    ''' Initializes a new instance of the <see cref="AgroEccezioni_ParticellaConLegami_Exception"/> class
    ''' with the specified error message and a reference to the inner
    ''' exception that is the cause of this exception.
    ''' </summary>
    ''' <param name="message">The message that describes the error.</param>
    ''' <param name="innerException">The exception that is the cause of the
    ''' current exception, or a null reference if no inner exception is
    ''' specified</param>
    Public Sub New(ByVal message As String, ByVal innerException As System.Exception)
        MyBase.New(message, innerException)
    End Sub

    ' Constructor required for serialization
    <SecuritySafeCritical()>
    Protected Sub New(ByVal info As SerializationInfo, ByVal context As StreamingContext)
        MyBase.New(info, context)
    End Sub

End Class

Public Enum ErroreGias_Tipo
    Generico = 0
    LoginFallito = 1
    ConflittoPermessi = 2
    ParticellaConLegami = 3
    NonGestito = 999
End Enum

Public Enum ErroreGias_Severity
    Bloccante = 0 'Errore in basso a dx rosso, l'utente non può procedere (sia errori gestiti che exceptions)
    Warning = 1 'Pop-up di n warning accodati con possibilità per l'utente di proseguire ("Si desidera proseguire?"   -   Annulla/Prosegui)
    Info = 2
    WarningBloccante = 3 'Pop-up di n errori accodati senza possibilità per l'utente di proseguire ("Non è possibile proseguire" - OK --> l'utente DEVE sistemare dei dati, NON può procedere altrimenti )
End Enum

Public Class ErroreGias
    Public severity As ErroreGias_Severity
    Public messaggio As String
    Public ex As String
    Public tipo As ErroreGias_Tipo
End Class