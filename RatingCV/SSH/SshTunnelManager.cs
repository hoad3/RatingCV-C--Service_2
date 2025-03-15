using Renci.SshNet;

namespace RatingCV.SSH;

public class SshTunnelManager
{
    private SshClient _sshClient;
    private ForwardedPortLocal _portForwarded;
    public int LocalPort { get; private set; }

    public SshTunnelManager(string sshHost, string sshUser, string sshPassword, string dbHost, int dbPort, int localPort = 5433)
    {
        _sshClient = new SshClient(sshHost, sshUser, sshPassword);
        _portForwarded = new ForwardedPortLocal("127.0.0.1", (uint)localPort, dbHost, (uint)dbPort);
        LocalPort = localPort;
    }

    public void StartTunnel()
    {
        _sshClient.Connect();
        _sshClient.AddForwardedPort(_portForwarded);
        _portForwarded.Start();
        Console.WriteLine($"SSH Tunnel established: 127.0.0.1:{LocalPort} -> Database");
    }

    public void StopTunnel()
    {
        if (_portForwarded != null && _portForwarded.IsStarted)
        {
            _portForwarded.Stop();
        }
        if (_sshClient != null && _sshClient.IsConnected)
        {
            _sshClient.Disconnect();
        }
        Console.WriteLine("SSH Tunnel closed.");
    }
}