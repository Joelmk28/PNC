using System.Linq;

namespace PNC.Models;

/// <summary>
/// Extensions pour la classe Policier
/// </summary>
public static class PolicierExtensions
{
    /// <summary>
    /// Vérifie si le policier a un statut de succès
    /// </summary>
    /// <param name="policier">Le policier à vérifier</param>
    /// <returns>True si le statut est un succès</returns>
    public static bool IsSuccess(this Policier policier)
    {
        return !string.IsNullOrEmpty(policier.Status) && 
               PolicierStatus.SuccessStatuses.Contains(policier.Status);
    }

    /// <summary>
    /// Vérifie si le policier a un statut d'échec
    /// </summary>
    /// <param name="policier">Le policier à vérifier</param>
    /// <returns>True si le statut est un échec</returns>
    public static bool IsFailure(this Policier policier)
    {
        return !string.IsNullOrEmpty(policier.Status) && 
               PolicierStatus.FailureStatuses.Contains(policier.Status);
    }

    /// <summary>
    /// Vérifie si le policier est en cours de traitement
    /// </summary>
    /// <param name="policier">Le policier à vérifier</param>
    /// <returns>True si le policier est en cours</returns>
    public static bool IsInProgress(this Policier policier)
    {
        return !string.IsNullOrEmpty(policier.Status) && 
               PolicierStatus.InProgressStatuses.Contains(policier.Status);
    }

    /// <summary>
    /// Vérifie si le policier a une photo capturée
    /// </summary>
    /// <param name="policier">Le policier à vérifier</param>
    /// <returns>True si la photo est capturée</returns>
    public static bool HasPhotoCaptured(this Policier policier)
    {
        return policier.Status == PolicierStatus.PhotoCapturee;
    }

    /// <summary>
    /// Vérifie si le policier a des empreintes capturées
    /// </summary>
    /// <param name="policier">Le policier à vérifier</param>
    /// <returns>True si les empreintes sont capturées</returns>
    public static bool HasFingerprintCaptured(this Policier policier)
    {
        return policier.Status == PolicierStatus.EmpreintesCapturees;
    }

    /// <summary>
    /// Vérifie si le policier a un document inséré
    /// </summary>
    /// <param name="policier">Le policier à vérifier</param>
    /// <returns>True si le document est inséré</returns>
    public static bool HasDocumentInserted(this Policier policier)
    {
        return policier.Status == PolicierStatus.DocumentInsere;
    }

    /// <summary>
    /// Vérifie si le policier est complet (photo + document + empreintes)
    /// </summary>
    /// <param name="policier">Le policier à vérifier</param>
    /// <returns>True si le policier est complet</returns>
    public static bool IsComplete(this Policier policier)
    {
        return policier.Status == PolicierStatus.Complet;
    }

    /// <summary>
    /// Vérifie si le policier est actif
    /// </summary>
    /// <param name="policier">Le policier à vérifier</param>
    /// <returns>True si le policier est actif</returns>
    public static bool IsActive(this Policier policier)
    {
        return policier.Status == PolicierStatus.Actif;
    }

    /// <summary>
    /// Obtient le libellé du statut en français
    /// </summary>
    /// <param name="policier">Le policier</param>
    /// <returns>Le libellé du statut</returns>
    public static string GetStatusLabel(this Policier policier)
    {
        return policier.Status switch
        {
            PolicierStatus.PhotoCapturee => "Photo Capturée",
            PolicierStatus.DocumentInsere => "Document Inséré",
            PolicierStatus.EmpreintesCapturees => "Empreintes Capturées",
            PolicierStatus.Complet => "Complet",
            PolicierStatus.EnCours => "En Cours",
            PolicierStatus.Echec => "Échec",
            PolicierStatus.EnAttente => "En Attente",
            PolicierStatus.Archive => "Archivé",
            PolicierStatus.Supprime => "Supprimé",
            PolicierStatus.Actif => "Actif",
            PolicierStatus.Inactif => "Inactif",
            _ => "Statut Inconnu"
        };
    }

    /// <summary>
    /// Obtient la classe CSS pour le statut
    /// </summary>
    /// <param name="policier">Le policier</param>
    /// <returns>La classe CSS</returns>
    public static string GetStatusCssClass(this Policier policier)
    {
        return policier.Status switch
        {
            PolicierStatus.PhotoCapturee => "status-success",
            PolicierStatus.DocumentInsere => "status-success",
            PolicierStatus.EmpreintesCapturees => "status-success",
            PolicierStatus.Complet => "status-success",
            PolicierStatus.EnCours => "status-warning",
            PolicierStatus.Echec => "status-danger",
            PolicierStatus.EnAttente => "status-info",
            PolicierStatus.Archive => "status-secondary",
            PolicierStatus.Supprime => "status-danger",
            PolicierStatus.Actif => "status-success",
            PolicierStatus.Inactif => "status-secondary",
            _ => "status-secondary"
        };
    }

    /// <summary>
    /// Obtient l'icône Bootstrap pour le statut
    /// </summary>
    /// <param name="policier">Le policier</param>
    /// <returns>La classe d'icône Bootstrap</returns>
    public static string GetStatusIcon(this Policier policier)
    {
        return policier.Status switch
        {
            PolicierStatus.PhotoCapturee => "bi-camera-fill",
            PolicierStatus.DocumentInsere => "bi-file-earmark-text-fill",
            PolicierStatus.EmpreintesCapturees => "bi-fingerprint",
            PolicierStatus.Complet => "bi-check-circle-fill",
            PolicierStatus.EnCours => "bi-clock-fill",
            PolicierStatus.Echec => "bi-x-circle-fill",
            PolicierStatus.EnAttente => "bi-hourglass-split",
            PolicierStatus.Archive => "bi-archive-fill",
            PolicierStatus.Supprime => "bi-trash-fill",
            PolicierStatus.Actif => "bi-person-check-fill",
            PolicierStatus.Inactif => "bi-person-x-fill",
            _ => "bi-question-circle-fill"
        };
    }

    /// <summary>
    /// Met à jour le statut du policier en fonction de ses documents
    /// </summary>
    /// <param name="policier">Le policier</param>
    public static void UpdateStatusFromDocuments(this Policier policier)
    {
        if (policier.Documents == null || !policier.Documents.Any())
        {
            policier.Status = PolicierStatus.EnAttente;
            return;
        }

        var documents = policier.Documents.Where(d => d.EstActif).ToList();
        
        if (!documents.Any())
        {
            policier.Status = PolicierStatus.EnAttente;
            return;
        }

        var hasPhoto = documents.Any(d => d.IsPhotoCaptured());
        var hasDocument = documents.Any(d => d.IsDocumentInserted());
        var hasFingerprint = documents.Any(d => d.IsFingerprintCaptured());

        if (hasPhoto && hasDocument && hasFingerprint)
        {
            policier.Status = PolicierStatus.Complet;
        }
        else if (hasFingerprint)
        {
            policier.Status = PolicierStatus.EmpreintesCapturees;
        }
        else if (hasDocument)
        {
            policier.Status = PolicierStatus.DocumentInsere;
        }
        else if (hasPhoto)
        {
            policier.Status = PolicierStatus.PhotoCapturee;
        }
        else
        {
            policier.Status = PolicierStatus.EnCours;
        }
    }
}
