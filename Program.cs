// See https://aka.ms/new-console-template for more information
using System.Security.Cryptography;
using System.Text;


    Console.OutputEncoding = Encoding.UTF8;
    Console.WriteLine("=== DEMO CRIPTOSISTEMA SIMÉTRICO (AES) \n");

    // Mensaje original
    var mensajeOriginal = "Información confidencial de la empresa";
    Console.WriteLine($"Texto original: {mensajeOriginal}\n");

    // Crear instancia de AES con clave y IV aleatorios
    using var aes = Aes.Create();
    aes.KeySize = 256; // AES-256
    aes.GenerateKey();
    aes.GenerateIV();

    // Guardamos la clave y IV
    var clave = aes.Key;
    var iv = aes.IV;

    // Cifrar
    var cifrado = CifrarAES(mensajeOriginal, clave, iv);
    Console.WriteLine($"Texto cifrado (Base64): {Convert.ToBase64String(cifrado)}\n");

    // Descifrar
    var descifrado = DescifrarAES(cifrado, clave, iv);
    Console.WriteLine($"Texto descifrado: {descifrado}\n");

    Console.WriteLine("Proceso completado ✅. Presiona una tecla para salir...");
    Console.ReadKey();


static byte[] CifrarAES(string textoPlano, byte[] clave, byte[] iv)
{
    using var aes = Aes.Create();
    aes.Key = clave;
    aes.IV = iv;

    // Crear transformador para cifrar
    var encryptor = aes.CreateEncryptor();

    using var ms = new MemoryStream();
    using var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);
    using var sw = new StreamWriter(cs, Encoding.UTF8);

    sw.Write(textoPlano);
    sw.Flush();
    cs.FlushFinalBlock();

    return ms.ToArray();
}

static string DescifrarAES(byte[] textoCifrado, byte[] clave, byte[] iv)
{
    using var aes = Aes.Create();
    aes.Key = clave;
    aes.IV = iv;

    var decryptor = aes.CreateDecryptor();

    using var ms = new MemoryStream(textoCifrado);
    using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
    using var sr = new StreamReader(cs, Encoding.UTF8);

    return sr.ReadToEnd();
}