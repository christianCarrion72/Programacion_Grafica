using System;
using OpenTK.Mathematics;

namespace proyectoPG
{
    public class Computadora
    {
        public static Escenario CrearEscenarioComputadora()
        {
            var escenario = new Escenario();
            
            // Crear los objetos individuales
            var monitorObj = CrearMonitor();
            var tecladoObj = CrearTeclado();
            var cpuObj = CrearCPU();
            var mesaObj = CrearMesa();
            
            // Añadir los objetos al escenario
            escenario.AddObjeto("monitor", monitorObj);
            escenario.AddObjeto("teclado", tecladoObj);
            escenario.AddObjeto("cpu", cpuObj);
            escenario.AddObjeto("mesa", mesaObj);
            
            return escenario;
        }
        
        public static Objeto CrearMonitor()
        {
            var monitor = new Objeto(0, 0, 0);
            var partePrincipal = new Parte(0, 0, 0);
            
            // Pantalla (frente) - Negro
            var pantalla = new Cara(0.0f, 0.0f, 0.0f);
            pantalla.AddVertice(-8f, 2f, 2f);
            pantalla.AddVertice(8f, 2f, 2f);
            pantalla.AddVertice(8f, 12f, 2f);
            pantalla.AddVertice(-8f, 12f, 2f);
            partePrincipal.AddCara(pantalla);

            // Marco del monitor (trasero) - Gris oscuro
            var marco = new Cara(0.3f, 0.3f, 0.3f);
            marco.AddVertice(-5.5f, 4.5f, -3f);
            marco.AddVertice(5.5f, 4.5f, -3f);
            marco.AddVertice(5.5f, 10.5f, -3f);
            marco.AddVertice(-5.5f, 10.5f, -3f);
            partePrincipal.AddCara(marco);

            // Laterales del monitor
            var lateralIzq = new Cara(0.4f, 0.4f, 0.4f);
            lateralIzq.AddVertice(-8f, 2f, 2f);
            lateralIzq.AddVertice(-5.5f, 4.5f, -3f);
            lateralIzq.AddVertice(-5.5f, 10.5f, -3f);
            lateralIzq.AddVertice(-8f, 12f, 2f);
            partePrincipal.AddCara(lateralIzq);

            var lateralDer = new Cara(0.4f, 0.4f, 0.4f);
            lateralDer.AddVertice(8f, 2f, 2f);
            lateralDer.AddVertice(5.5f, 4.5f, -3f);
            lateralDer.AddVertice(5.5f, 10.5f, -3f);
            lateralDer.AddVertice(8f, 12f, 2f);
            partePrincipal.AddCara(lateralDer);

            var lateralInf = new Cara(0.4f, 0.4f, 0.4f);
            lateralInf.AddVertice(-8f, 2f, 2f);
            lateralInf.AddVertice(8f, 2f, 2f);
            lateralInf.AddVertice(5.5f, 4.5f, -3f);
            lateralInf.AddVertice(-5.5f, 4.5f, -3f);
            partePrincipal.AddCara(lateralInf);

            var lateralSup = new Cara(0.4f, 0.4f, 0.4f);
            lateralSup.AddVertice(-8f, 12f, 2f);
            lateralSup.AddVertice(8f, 12f, 2f);
            lateralSup.AddVertice(5.5f, 10.5f, -3f);
            lateralSup.AddVertice(-5.5f, 10.5f, -3f);
            partePrincipal.AddCara(lateralSup);

            // Base del monitor - Gris
            var baseMonitor = new Cara(0.5f, 0.5f, 0.5f);
            baseMonitor.AddVertice(-2f, 0f, -1f);
            baseMonitor.AddVertice(2f, 0f, -1f);
            baseMonitor.AddVertice(2f, 4f, -1f);
            baseMonitor.AddVertice(-2f, 4f, -1f);
            partePrincipal.AddCara(baseMonitor);

            var baseMonitor2 = new Cara(0.5f, 0.5f, 0.5f);
            baseMonitor2.AddVertice(-2f, 0f, 3f);
            baseMonitor2.AddVertice(2f, 0f, 3f);
            baseMonitor2.AddVertice(2f, 1.5f, 3f);
            baseMonitor2.AddVertice(-2f, 1.5f, 3f);
            partePrincipal.AddCara(baseMonitor2);

            // Laterales de la base
            var baseLateralIzq = new Cara(0.5f, 0.5f, 0.5f);
            baseLateralIzq.AddVertice(-2f, 0f, -1f);
            baseLateralIzq.AddVertice(-2f, 4f, -1f);
            baseLateralIzq.AddVertice(-2f, 1.5f, 3f);
            baseLateralIzq.AddVertice(-2f, 0f, 3f);
            partePrincipal.AddCara(baseLateralIzq);

            var baseLateralDer = new Cara(0.5f, 0.5f, 0.5f);
            baseLateralDer.AddVertice(2f, 0f, -1f);
            baseLateralDer.AddVertice(2f, 4f, -1f);
            baseLateralDer.AddVertice(2f, 1.5f, 3f);
            baseLateralDer.AddVertice(2f, 0f, 3f);
            partePrincipal.AddCara(baseLateralDer);

            var baseFrente = new Cara(0.5f, 0.5f, 0.5f);
            baseFrente.AddVertice(-2f, 0f, -1f);
            baseFrente.AddVertice(2f, 0f, -1f);
            baseFrente.AddVertice(2f, 4f, -1f);
            baseFrente.AddVertice(-2f, 4f, -1f);
            partePrincipal.AddCara(baseFrente);

            var baseAtras = new Cara(0.5f, 0.5f, 0.5f);
            baseAtras.AddVertice(-2f, 0f, 3f);
            baseAtras.AddVertice(2f, 0f, 3f);
            baseAtras.AddVertice(2f, 1.5f, 3f);
            baseAtras.AddVertice(-2f, 1.5f, 3f);
            partePrincipal.AddCara(baseAtras);
            
            monitor.AddParte("monitor", partePrincipal);
            return monitor;
        }
        
        public static Objeto CrearTeclado()
        {
            var teclado = new Objeto(0, 0.1f, 6f);
            var partePrincipal = new Parte(0, 0, 0);
            
            // Teclado principal - Blanco/Gris claro
            var tecladoSuperior = new Cara(0.9f, 0.9f, 0.9f);
            tecladoSuperior.AddVertice(-6f, 0.2f, 4f);
            tecladoSuperior.AddVertice(6f, 0.2f, 4f);
            tecladoSuperior.AddVertice(6f, 0.2f, 8f);
            tecladoSuperior.AddVertice(-6f, 0.2f, 8f);
            partePrincipal.AddCara(tecladoSuperior);

            var tecladoInferior = new Cara(0.8f, 0.8f, 0.8f);
            tecladoInferior.AddVertice(-6f, 0f, 4f);
            tecladoInferior.AddVertice(6f, 0f, 4f);
            tecladoInferior.AddVertice(6f, 0f, 8f);
            tecladoInferior.AddVertice(-6f, 0f, 8f);
            partePrincipal.AddCara(tecladoInferior);

            // Laterales del teclado
            var tecladoLateralIzq = new Cara(0.85f, 0.85f, 0.85f);
            tecladoLateralIzq.AddVertice(-6f, 0f, 4f);
            tecladoLateralIzq.AddVertice(-6f, 0.2f, 4f);
            tecladoLateralIzq.AddVertice(-6f, 0.2f, 8f);
            tecladoLateralIzq.AddVertice(-6f, 0f, 8f);
            partePrincipal.AddCara(tecladoLateralIzq);

            var tecladoLateralDer = new Cara(0.85f, 0.85f, 0.85f);
            tecladoLateralDer.AddVertice(6f, 0f, 4f);
            tecladoLateralDer.AddVertice(6f, 0.2f, 4f);
            tecladoLateralDer.AddVertice(6f, 0.2f, 8f);
            tecladoLateralDer.AddVertice(6f, 0f, 8f);
            partePrincipal.AddCara(tecladoLateralDer);

            var tecladoFrente = new Cara(0.85f, 0.85f, 0.85f);
            tecladoFrente.AddVertice(-6f, 0f, 4f);
            tecladoFrente.AddVertice(6f, 0f, 4f);
            tecladoFrente.AddVertice(6f, 0.2f, 4f);
            tecladoFrente.AddVertice(-6f, 0.2f, 4f);
            partePrincipal.AddCara(tecladoFrente);

            var tecladoAtras = new Cara(0.85f, 0.85f, 0.85f);
            tecladoAtras.AddVertice(-6f, 0f, 8f);
            tecladoAtras.AddVertice(6f, 0f, 8f);
            tecladoAtras.AddVertice(6f, 0.2f, 8f);
            tecladoAtras.AddVertice(-6f, 0.2f, 8f);
            partePrincipal.AddCara(tecladoAtras);
            
            teclado.AddParte("teclado", partePrincipal);
            return teclado;
        }
        
        public static Objeto CrearCPU()
        {
            var cpu = new Objeto(1, 0, 1);
            var partePrincipal = new Parte(0, 0, 0);
            
            // Frente de la CPU - Negro/Gris oscuro
            var cpuFrente = new Cara(0.2f, 0.2f, 0.2f);
            cpuFrente.AddVertice(12f, 0f, -3f);
            cpuFrente.AddVertice(16f, 0f, -3f);
            cpuFrente.AddVertice(16f, 12f, -3f);
            cpuFrente.AddVertice(12f, 12f, -3f);
            partePrincipal.AddCara(cpuFrente);

            // Trasero de la CPU
            var cpuTrasero = new Cara(0.15f, 0.15f, 0.15f);
            cpuTrasero.AddVertice(12f, 0f, 5f);
            cpuTrasero.AddVertice(16f, 0f, 5f);
            cpuTrasero.AddVertice(16f, 12f, 5f);
            cpuTrasero.AddVertice(12f, 12f, 5f);
            partePrincipal.AddCara(cpuTrasero);

            // Laterales de la CPU
            var cpuLateralIzq = new Cara(0.18f, 0.18f, 0.18f);
            cpuLateralIzq.AddVertice(12f, 0f, -3f);
            cpuLateralIzq.AddVertice(12f, 0f, 5f);
            cpuLateralIzq.AddVertice(12f, 12f, 5f);
            cpuLateralIzq.AddVertice(12f, 12f, -3f);
            partePrincipal.AddCara(cpuLateralIzq);

            var cpuLateralDer = new Cara(0.18f, 0.18f, 0.18f);
            cpuLateralDer.AddVertice(16f, 0f, -3f);
            cpuLateralDer.AddVertice(16f, 0f, 5f);
            cpuLateralDer.AddVertice(16f, 12f, 5f);
            cpuLateralDer.AddVertice(16f, 12f, -3f);
            partePrincipal.AddCara(cpuLateralDer);

            var cpuInferior = new Cara(0.18f, 0.18f, 0.18f);
            cpuInferior.AddVertice(12f, 0f, -3f);
            cpuInferior.AddVertice(16f, 0f, -3f);
            cpuInferior.AddVertice(16f, 0f, 5f);
            cpuInferior.AddVertice(12f, 0f, 5f);
            partePrincipal.AddCara(cpuInferior);

            var cpuSuperior = new Cara(0.18f, 0.18f, 0.18f);
            cpuSuperior.AddVertice(12f, 12f, -3f);
            cpuSuperior.AddVertice(16f, 12f, -3f);
            cpuSuperior.AddVertice(16f, 12f, 5f);
            cpuSuperior.AddVertice(12f, 12f, 5f);
            partePrincipal.AddCara(cpuSuperior);
            
            cpu.AddParte("cpu", partePrincipal);
            return cpu;
        }
        
        public static Objeto CrearMesa()
        {
            var mesa = new Objeto(0, 0.60f, 2.5f);
            var partePrincipal = new Parte(0, 0, 0);
            
            // Mesa - Marrón claro
            var mesaSuperior = new Cara(0.6f, 0.4f, 0.2f);
            mesaSuperior.AddVertice(-20f, -0.5f, -10f);
            mesaSuperior.AddVertice(20f, -0.5f, -10f);
            mesaSuperior.AddVertice(20f, -0.5f, 15f);
            mesaSuperior.AddVertice(-20f, -0.5f, 15f);
            partePrincipal.AddCara(mesaSuperior);

            var mesaInferior = new Cara(0.5f, 0.3f, 0.15f);
            mesaInferior.AddVertice(-20f, -1f, -10f);
            mesaInferior.AddVertice(20f, -1f, -10f);
            mesaInferior.AddVertice(20f, -1f, 15f);
            mesaInferior.AddVertice(-20f, -1f, 15f);
            partePrincipal.AddCara(mesaInferior);

            // Laterales de la mesa
            var mesaLateralIzq = new Cara(0.55f, 0.35f, 0.175f);
            mesaLateralIzq.AddVertice(-20f, -1f, -10f);
            mesaLateralIzq.AddVertice(-20f, -0.5f, -10f);
            mesaLateralIzq.AddVertice(-20f, -0.5f, 15f);
            mesaLateralIzq.AddVertice(-20f, -1f, 15f);
            partePrincipal.AddCara(mesaLateralIzq);

            var mesaLateralDer = new Cara(0.55f, 0.35f, 0.175f);
            mesaLateralDer.AddVertice(20f, -1f, -10f);
            mesaLateralDer.AddVertice(20f, -0.5f, -10f);
            mesaLateralDer.AddVertice(20f, -0.5f, 15f);
            mesaLateralDer.AddVertice(20f, -1f, 15f);
            partePrincipal.AddCara(mesaLateralDer);

            var mesaFrente = new Cara(0.55f, 0.35f, 0.175f);
            mesaFrente.AddVertice(-20f, -1f, -10f);
            mesaFrente.AddVertice(20f, -1f, -10f);
            mesaFrente.AddVertice(20f, -0.5f, -10f);
            mesaFrente.AddVertice(-20f, -0.5f, -10f);
            partePrincipal.AddCara(mesaFrente);

            var mesaAtras = new Cara(0.55f, 0.35f, 0.175f);
            mesaAtras.AddVertice(-20f, -1f, 15f);
            mesaAtras.AddVertice(20f, -1f, 15f);
            mesaAtras.AddVertice(20f, -0.5f, 15f);
            mesaAtras.AddVertice(-20f, -0.5f, 15f);
            partePrincipal.AddCara(mesaAtras);
            
            mesa.AddParte("mesa", partePrincipal);
            return mesa;
        }
        
        public static Objeto CrearComputadora()
        {
            var computadora = new Objeto(0, 0, 0);

            // ===== MONITOR =====
            var monitor = new Parte(0, 0, 0); // Centro del monitor

            // Pantalla (frente) - Negro
            var pantalla = new Cara(0.0f, 0.0f, 0.0f);
            pantalla.AddVertice(-8f, 2f, 2f);
            pantalla.AddVertice(8f, 2f, 2f);
            pantalla.AddVertice(8f, 12f, 2f);
            pantalla.AddVertice(-8f, 12f, 2f);
            monitor.AddCara(pantalla);

            // Marco del monitor (trasero) - Gris oscuro
            var marco = new Cara(0.3f, 0.3f, 0.3f);
            marco.AddVertice(-5.5f, 4.5f, -3f);
            marco.AddVertice(5.5f, 4.5f, -3f);
            marco.AddVertice(5.5f, 10.5f, -3f);
            marco.AddVertice(-5.5f, 10.5f, -3f);
            monitor.AddCara(marco);

            // Laterales del monitor
            var lateralIzq = new Cara(0.4f, 0.4f, 0.4f);
            lateralIzq.AddVertice(-8f, 2f, 2f);
            lateralIzq.AddVertice(-5.5f, 4.5f, -3f);
            lateralIzq.AddVertice(-5.5f, 10.5f, -3f);
            lateralIzq.AddVertice(-8f, 12f, 2f);
            monitor.AddCara(lateralIzq);

            var lateralDer = new Cara(0.4f, 0.4f, 0.4f);
            lateralDer.AddVertice(8f, 2f, 2f);
            lateralDer.AddVertice(5.5f, 4.5f, -3f);
            lateralDer.AddVertice(5.5f, 10.5f, -3f);
            lateralDer.AddVertice(8f, 12f, 2f);
            monitor.AddCara(lateralDer);

            var lateralInf = new Cara(0.4f, 0.4f, 0.4f);
            lateralInf.AddVertice(-8f, 2f, 2f);
            lateralInf.AddVertice(8f, 2f, 2f);
            lateralInf.AddVertice(5.5f, 4.5f, -3f);
            lateralInf.AddVertice(-5.5f, 4.5f, -3f);
            monitor.AddCara(lateralInf);

            var lateralSup = new Cara(0.4f, 0.4f, 0.4f);
            lateralSup.AddVertice(-8f, 12f, 2f);
            lateralSup.AddVertice(8f, 12f, 2f);
            lateralSup.AddVertice(5.5f, 10.5f, -3f);
            lateralSup.AddVertice(-5.5f, 10.5f, -3f);
            monitor.AddCara(lateralSup);

            // Base del monitor - Gris
            var baseMonitor = new Cara(0.5f, 0.5f, 0.5f);
            baseMonitor.AddVertice(-2f, 0f, -1f);
            baseMonitor.AddVertice(2f, 0f, -1f);
            baseMonitor.AddVertice(2f, 4f, -1f);
            baseMonitor.AddVertice(-2f, 4f, -1f);
            monitor.AddCara(baseMonitor);

            var baseMonitor2 = new Cara(0.5f, 0.5f, 0.5f);
            baseMonitor2.AddVertice(-2f, 0f, 3f);
            baseMonitor2.AddVertice(2f, 0f, 3f);
            baseMonitor2.AddVertice(2f, 1.5f, 3f);
            baseMonitor2.AddVertice(-2f, 1.5f, 3f);
            monitor.AddCara(baseMonitor2);

            // Laterales de la base
            var baseLateralIzq = new Cara(0.5f, 0.5f, 0.5f);
            baseLateralIzq.AddVertice(-2f, 0f, -1f);
            baseLateralIzq.AddVertice(-2f, 4f, -1f);
            baseLateralIzq.AddVertice(-2f, 1.5f, 3f);
            baseLateralIzq.AddVertice(-2f, 0f, 3f);
            monitor.AddCara(baseLateralIzq);

            var baseLateralDer = new Cara(0.5f, 0.5f, 0.5f);
            baseLateralDer.AddVertice(2f, 0f, -1f);
            baseLateralDer.AddVertice(2f, 4f, -1f);
            baseLateralDer.AddVertice(2f, 1.5f, 3f);
            baseLateralDer.AddVertice(2f, 0f, 3f);
            monitor.AddCara(baseLateralDer);

            var baseFrente = new Cara(0.5f, 0.5f, 0.5f);
            baseFrente.AddVertice(-2f, 0f, -1f);
            baseFrente.AddVertice(2f, 0f, -1f);
            baseFrente.AddVertice(2f, 4f, -1f);
            baseFrente.AddVertice(-2f, 4f, -1f);
            monitor.AddCara(baseFrente);

            var baseAtras = new Cara(0.5f, 0.5f, 0.5f);
            baseAtras.AddVertice(-2f, 0f, 3f);
            baseAtras.AddVertice(2f, 0f, 3f);
            baseAtras.AddVertice(2f, 1.5f, 3f);
            baseAtras.AddVertice(-2f, 1.5f, 3f);
            monitor.AddCara(baseAtras);

            computadora.AddParte("monitor", monitor);

            // ===== TECLADO =====
            var teclado = new Parte(0, 0.1f, 6f); // Centro del teclado

            // Teclado principal - Blanco/Gris claro
            var tecladoSuperior = new Cara(0.9f, 0.9f, 0.9f);
            tecladoSuperior.AddVertice(-6f, 0.2f, 4f);
            tecladoSuperior.AddVertice(6f, 0.2f, 4f);
            tecladoSuperior.AddVertice(6f, 0.2f, 8f);
            tecladoSuperior.AddVertice(-6f, 0.2f, 8f);
            teclado.AddCara(tecladoSuperior);

            var tecladoInferior = new Cara(0.8f, 0.8f, 0.8f);
            tecladoInferior.AddVertice(-6f, 0f, 4f);
            tecladoInferior.AddVertice(6f, 0f, 4f);
            tecladoInferior.AddVertice(6f, 0f, 8f);
            tecladoInferior.AddVertice(-6f, 0f, 8f);
            teclado.AddCara(tecladoInferior);

            // Laterales del teclado
            var tecladoLateralIzq = new Cara(0.85f, 0.85f, 0.85f);
            tecladoLateralIzq.AddVertice(-6f, 0f, 4f);
            tecladoLateralIzq.AddVertice(-6f, 0.2f, 4f);
            tecladoLateralIzq.AddVertice(-6f, 0.2f, 8f);
            tecladoLateralIzq.AddVertice(-6f, 0f, 8f);
            teclado.AddCara(tecladoLateralIzq);

            var tecladoLateralDer = new Cara(0.85f, 0.85f, 0.85f);
            tecladoLateralDer.AddVertice(6f, 0f, 4f);
            tecladoLateralDer.AddVertice(6f, 0.2f, 4f);
            tecladoLateralDer.AddVertice(6f, 0.2f, 8f);
            tecladoLateralDer.AddVertice(6f, 0f, 8f);
            teclado.AddCara(tecladoLateralDer);

            var tecladoFrente = new Cara(0.85f, 0.85f, 0.85f);
            tecladoFrente.AddVertice(-6f, 0f, 4f);
            tecladoFrente.AddVertice(6f, 0f, 4f);
            tecladoFrente.AddVertice(6f, 0.2f, 4f);
            tecladoFrente.AddVertice(-6f, 0.2f, 4f);
            teclado.AddCara(tecladoFrente);

            var tecladoAtras = new Cara(0.85f, 0.85f, 0.85f);
            tecladoAtras.AddVertice(-6f, 0f, 8f);
            tecladoAtras.AddVertice(6f, 0f, 8f);
            tecladoAtras.AddVertice(6f, 0.2f, 8f);
            tecladoAtras.AddVertice(-6f, 0.2f, 8f);
            teclado.AddCara(tecladoAtras);

            computadora.AddParte("teclado", teclado);

            // ===== CPU =====
            var cpu = new Parte(1, 0, 1); // Centro de la CPU

            // Frente de la CPU - Negro/Gris oscuro
            var cpuFrente = new Cara(0.2f, 0.2f, 0.2f);
            cpuFrente.AddVertice(12f, 0f, -3f);
            cpuFrente.AddVertice(16f, 0f, -3f);
            cpuFrente.AddVertice(16f, 12f, -3f);
            cpuFrente.AddVertice(12f, 12f, -3f);
            cpu.AddCara(cpuFrente);

            // Trasero de la CPU
            var cpuTrasero = new Cara(0.15f, 0.15f, 0.15f);
            cpuTrasero.AddVertice(12f, 0f, 5f);
            cpuTrasero.AddVertice(16f, 0f, 5f);
            cpuTrasero.AddVertice(16f, 12f, 5f);
            cpuTrasero.AddVertice(12f, 12f, 5f);
            cpu.AddCara(cpuTrasero);

            // Laterales de la CPU
            var cpuLateralIzq = new Cara(0.18f, 0.18f, 0.18f);
            cpuLateralIzq.AddVertice(12f, 0f, -3f);
            cpuLateralIzq.AddVertice(12f, 0f, 5f);
            cpuLateralIzq.AddVertice(12f, 12f, 5f);
            cpuLateralIzq.AddVertice(12f, 12f, -3f);
            cpu.AddCara(cpuLateralIzq);

            var cpuLateralDer = new Cara(0.18f, 0.18f, 0.18f);
            cpuLateralDer.AddVertice(16f, 0f, -3f);
            cpuLateralDer.AddVertice(16f, 0f, 5f);
            cpuLateralDer.AddVertice(16f, 12f, 5f);
            cpuLateralDer.AddVertice(16f, 12f, -3f);
            cpu.AddCara(cpuLateralDer);

            var cpuInferior = new Cara(0.18f, 0.18f, 0.18f);
            cpuInferior.AddVertice(12f, 0f, -3f);
            cpuInferior.AddVertice(16f, 0f, -3f);
            cpuInferior.AddVertice(16f, 0f, 5f);
            cpuInferior.AddVertice(12f, 0f, 5f);
            cpu.AddCara(cpuInferior);

            var cpuSuperior = new Cara(0.18f, 0.18f, 0.18f);
            cpuSuperior.AddVertice(12f, 12f, -3f);
            cpuSuperior.AddVertice(16f, 12f, -3f);
            cpuSuperior.AddVertice(16f, 12f, 5f);
            cpuSuperior.AddVertice(12f, 12f, 5f);
            cpu.AddCara(cpuSuperior);

            computadora.AddParte("cpu", cpu);

            // ===== MESA =====
            var mesa = new Parte(0, 0.60f, 2.5f); // Centro de la mesa

            // Mesa - Marrón claro
            var mesaSuperior = new Cara(0.6f, 0.4f, 0.2f);
            mesaSuperior.AddVertice(-20f, -0.5f, -10f);
            mesaSuperior.AddVertice(20f, -0.5f, -10f);
            mesaSuperior.AddVertice(20f, -0.5f, 15f);
            mesaSuperior.AddVertice(-20f, -0.5f, 15f);
            mesa.AddCara(mesaSuperior);

            var mesaInferior = new Cara(0.5f, 0.3f, 0.15f);
            mesaInferior.AddVertice(-20f, -1f, -10f);
            mesaInferior.AddVertice(20f, -1f, -10f);
            mesaInferior.AddVertice(20f, -1f, 15f);
            mesaInferior.AddVertice(-20f, -1f, 15f);
            mesa.AddCara(mesaInferior);

            // Laterales de la mesa
            var mesaLateralIzq = new Cara(0.55f, 0.35f, 0.175f);
            mesaLateralIzq.AddVertice(-20f, -1f, -10f);
            mesaLateralIzq.AddVertice(-20f, -0.5f, -10f);
            mesaLateralIzq.AddVertice(-20f, -0.5f, 15f);
            mesaLateralIzq.AddVertice(-20f, -1f, 15f);
            mesa.AddCara(mesaLateralIzq);

            var mesaLateralDer = new Cara(0.55f, 0.35f, 0.175f);
            mesaLateralDer.AddVertice(20f, -1f, -10f);
            mesaLateralDer.AddVertice(20f, -0.5f, -10f);
            mesaLateralDer.AddVertice(20f, -0.5f, 15f);
            mesaLateralDer.AddVertice(20f, -1f, 15f);
            mesa.AddCara(mesaLateralDer);

            var mesaFrente = new Cara(0.55f, 0.35f, 0.175f);
            mesaFrente.AddVertice(-20f, -1f, -10f);
            mesaFrente.AddVertice(20f, -1f, -10f);
            mesaFrente.AddVertice(20f, -0.5f, -10f);
            mesaFrente.AddVertice(-20f, -0.5f, -10f);
            mesa.AddCara(mesaFrente);

            var mesaAtras = new Cara(0.55f, 0.35f, 0.175f);
            mesaAtras.AddVertice(-20f, -1f, 15f);
            mesaAtras.AddVertice(20f, -1f, 15f);
            mesaAtras.AddVertice(20f, -0.5f, 15f);
            mesaAtras.AddVertice(-20f, -0.5f, 15f);
            mesa.AddCara(mesaAtras);

            computadora.AddParte("mesa", mesa);

            return computadora;
        }
    }
}