namespace PNC.Models;

/// <summary>
/// Énumération des statuts possibles pour les policiers
/// </summary>
public static class PolicierStatus
{
    /// <summary>
    /// Policier avec photo capturée
    /// </summary>
    public const string PhotoCapturee = "Photo_Capturee";

    /// <summary>
    /// Policier avec document inséré
    /// </summary>
    public const string DocumentInsere = "Document_Insere";

    /// <summary>
    /// Policier avec empreintes capturées
    /// </summary>
    public const string EmpreintesCapturees = "Empreintes_Capturees";

    /// <summary>
    /// Policier complet (photo + document + empreintes)
    /// </summary>
    public const string Complet = "Complet";

    /// <summary>
    /// Policier en cours de traitement
    /// </summary>
    public const string EnCours = "En_Cours";

    /// <summary>
    /// Policier en attente de traitement
    /// </summary>
    public const string EnAttente = "En_Attente";

    /// <summary>
    /// Policier avec échec de traitement
    /// </summary>
    public const string Echec = "Echec";

    /// <summary>
    /// Policier archivé
    /// </summary>
    public const string Archive = "Archive";

    /// <summary>
    /// Policier supprimé
    /// </summary>
    public const string Supprime = "Supprime";

    /// <summary>
    /// Policier actif
    /// </summary>
    public const string Actif = "Actif";

    /// <summary>
    /// Policier inactif
    /// </summary>
    public const string Inactif = "Inactif";

    /// <summary>
    /// Obtient tous les statuts disponibles
    /// </summary>
    public static readonly string[] AllStatuses = new[]
    {
        PhotoCapturee,
        DocumentInsere,
        EmpreintesCapturees,
        Complet,
        EnCours,
        EnAttente,
        Echec,
        Archive,
        Supprime,
        Actif,
        Inactif
    };

    /// <summary>
    /// Obtient les statuts de succès
    /// </summary>
    public static readonly string[] SuccessStatuses = new[]
    {
        PhotoCapturee,
        DocumentInsere,
        EmpreintesCapturees,
        Complet,
        Actif
    };

    /// <summary>
    /// Obtient les statuts d'échec
    /// </summary>
    public static readonly string[] FailureStatuses = new[]
    {
        Echec,
        Supprime,
        Inactif
    };

    /// <summary>
    /// Obtient les statuts en cours
    /// </summary>
    public static readonly string[] InProgressStatuses = new[]
    {
        EnCours,
        EnAttente
    };

    /// <summary>
    /// Obtient les statuts de capture
    /// </summary>
    public static readonly string[] CaptureStatuses = new[]
    {
        PhotoCapturee,
        DocumentInsere,
        EmpreintesCapturees,
        Complet
    };
}
