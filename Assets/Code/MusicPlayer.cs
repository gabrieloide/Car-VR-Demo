using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MusicPlayer : MonoBehaviour
{
    [Header("Referencias de UI")]
    public Button playPauseButton;
    public Button nextButton;
    public Button previousButton;
    public TMP_Text songTitleText; // Opcional: para mostrar el nombre de la canción
    public Slider progressSlider; // Opcional: para mostrar progreso
    
    [Header("Configuración")]
    public string musicFolderPath = "Music"; // Carpeta dentro de Resources
    
    private AudioSource audioSource;
    private List<AudioClip> musicPlaylist;
    private int currentSongIndex = 0;
    private bool isPlaying = false;
    private bool isPaused = false;
    
    void Start()
    {
        // Obtener o crear AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        
        // Configurar AudioSource
        audioSource.loop = false; // No loop individual, manejamos el loop manualmente
        audioSource.playOnAwake = false;
        
        // Cargar todas las canciones
        LoadAllMusic();
        
        // Configurar botones
        SetupButtons();
        
        // Reproducir la primera canción
        if (musicPlaylist.Count > 0)
        {
            PlayCurrentSong();
        }
        else
        {
            Debug.LogWarning("No se encontraron canciones en la carpeta: " + musicFolderPath);
        }
    }
    
    void Update()
    {
        // Verificar si la canción actual terminó para pasar a la siguiente
        if (isPlaying && !audioSource.isPlaying && !isPaused)
        {
            NextSong();
        }
        
        // Actualizar slider de progreso (opcional)
        UpdateProgressSlider();
    }
    
    void LoadAllMusic()
    {
        musicPlaylist = new List<AudioClip>();
        
        // Cargar todos los AudioClips desde Resources
        AudioClip[] clips = Resources.LoadAll<AudioClip>(musicFolderPath);
        
        if (clips.Length > 0)
        {
            musicPlaylist.AddRange(clips);
            Debug.Log("Canciones cargadas: " + musicPlaylist.Count);
            
            // Mostrar nombres de las canciones cargadas
            for (int i = 0; i < musicPlaylist.Count; i++)
            {
                Debug.Log($"Canción {i + 1}: {musicPlaylist[i].name}");
            }
        }
        else
        {
            Debug.LogError("No se encontraron archivos de audio en Resources/" + musicFolderPath);
        }
    }
    
    void SetupButtons()
    {
        if (playPauseButton != null)
            playPauseButton.onClick.AddListener(TogglePlayPause);
            
        if (nextButton != null)
            nextButton.onClick.AddListener(NextSong);
            
        if (previousButton != null)
            previousButton.onClick.AddListener(PreviousSong);
    }
    
    public void TogglePlayPause()
    {
        if (musicPlaylist.Count == 0) return;
        
        if (isPlaying)
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
        else
        {
            PlayCurrentSong();
        }
    }
    
    public void Pause()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Pause();
            isPaused = true;
            Debug.Log("Música pausada");
            
            // Cambiar texto del botón (opcional)
            if (playPauseButton != null)
            {
                Text buttonText = playPauseButton.GetComponentInChildren<Text>();
                if (buttonText != null)
                    buttonText.text = "▶";
            }
        }
    }
    
    public void Resume()
    {
        if (isPaused)
        {
            audioSource.UnPause();
            isPaused = false;
            Debug.Log("Música reanudada");
            
            // Cambiar texto del botón (opcional)
            if (playPauseButton != null)
            {
                Text buttonText = playPauseButton.GetComponentInChildren<Text>();
                if (buttonText != null)
                    buttonText.text = "⏸";
            }
        }
    }
    
    public void NextSong()
    {
        if (musicPlaylist.Count == 0) return;
        
        // Avanzar al siguiente índice (con bucle)
        currentSongIndex = (currentSongIndex + 1) % musicPlaylist.Count;
        PlayCurrentSong();
        Debug.Log("Siguiente canción: " + musicPlaylist[currentSongIndex].name);
    }
    
    public void PreviousSong()
    {
        if (musicPlaylist.Count == 0) return;
        
        // Retroceder al índice anterior (con bucle)
        currentSongIndex--;
        if (currentSongIndex < 0)
            currentSongIndex = musicPlaylist.Count - 1;
            
        PlayCurrentSong();
        Debug.Log("Canción anterior: " + musicPlaylist[currentSongIndex].name);
    }
    
    void PlayCurrentSong()
    {
        if (musicPlaylist.Count == 0) return;
        
        audioSource.clip = musicPlaylist[currentSongIndex];
        audioSource.Play();
        isPlaying = true;
        isPaused = false;
        
        Debug.Log("Reproduciendo: " + musicPlaylist[currentSongIndex].name);
        
        // Actualizar UI
        UpdateSongTitle();
        
        // Cambiar texto del botón (opcional)
        if (playPauseButton != null)
        {
            Text buttonText = playPauseButton.GetComponentInChildren<Text>();
            if (buttonText != null)
                buttonText.text = "⏸";
        }
    }
    
    void UpdateSongTitle()
    {
        if (songTitleText != null && musicPlaylist.Count > 0)
        {
            songTitleText.text = musicPlaylist[currentSongIndex].name;
        }
    }
    
    void UpdateProgressSlider()
    {
        if (progressSlider != null && audioSource.clip != null)
        {
            progressSlider.value = audioSource.time / audioSource.clip.length;
        }
    }
    
    // Método público para obtener información del reproductor
    public string GetCurrentSongName()
    {
        if (musicPlaylist.Count > 0)
            return musicPlaylist[currentSongIndex].name;
        return "Sin canción";
    }
    
    public int GetCurrentSongIndex()
    {
        return currentSongIndex;
    }
    
    public int GetTotalSongs()
    {
        return musicPlaylist.Count;
    }
    
    public bool IsPlaying()
    {
        return isPlaying && !isPaused;
    }
    
    public bool IsPaused()
    {
        return isPaused;
    }
    
    // Método para saltar a una canción específica
    public void PlaySongAtIndex(int index)
    {
        if (index >= 0 && index < musicPlaylist.Count)
        {
            currentSongIndex = index;
            PlayCurrentSong();
        }
    }
}