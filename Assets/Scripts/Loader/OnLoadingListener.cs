 public interface OnLoadingListener {
    

    void Start();

    void Progress(float progress, string message);
    void End();

    void Error(float progress, string message);

    
}