import { useCallback, useEffect, useMemo, useState } from 'react'
import axios from 'axios'
import './App.css'

const API_URL = import.meta.env.VITE_API_URL || 'http://localhost:5096/api'

const municipios = [
  'Medellin',
  'Envigado',
  'Itagui',
  'Sabaneta',
  'Bello',
  'Caldas',
  'La Estrella',
  'Copacabana',
  'Girardota',
  'Barbosa',
]

const estados = ['ENVIADO', 'ENTREGADO', 'CANCELADO']
const ALERT_TIMEOUTS = { error: 2000, ok: 2800 }
const TRACKING_PAGE_SIZE = 8

const empleadoVacio = {
  documentoIdentidad: '',
  nombreUsuario: '',
  nombre: '',
  correo: '',
  telefono: '',
  direccion: '',
  contrasena: '',
  rol: 'EMPLEADO',
  activo: true,
}

const envioVacio = {
  clienteDocumento: '',
  clienteNombre: '',
  clienteCorreo: '',
  clienteTelefono: '',
  clienteDireccion: '',
  descripcion: '',
  municipioDestino: 'Medellin',
  peso: 1,
  cantidad: 1,
  direccion: '',
  esDelicado: false,
}

function App() {
  const [sesion, setSesion] = useState(() => {
    const guardada = localStorage.getItem('suenvioseguro.session')
    return guardada ? JSON.parse(guardada) : null
  })
  const [vista, setVista] = useState('facturacion')
  const [alerta, setAlerta] = useState(null)

  const api = useMemo(() => {
    const instancia = axios.create({ baseURL: API_URL })
    instancia.interceptors.request.use((config) => {
      if (sesion?.token) {
        config.headers.Authorization = `Bearer ${sesion.token}`
      }
      return config
    })
    return instancia
  }, [sesion])

  const guardarSesion = (datos) => {
    setAlerta(null)
    setSesion(datos)
    localStorage.setItem('suenvioseguro.session', JSON.stringify(datos))
  }

  const cerrarSesion = () => {
    setSesion(null)
    localStorage.removeItem('suenvioseguro.session')
    setVista('facturacion')
  }

  const mostrarError = useCallback((error) => {
    const detalle = error?.response?.data?.detail
      || error?.response?.data?.title
      || error?.response?.data?.message
      || 'No fue posible completar la operacion.'
    setAlerta({ tipo: 'error', texto: detalle })
  }, [])

  const mostrarOk = useCallback((texto) => setAlerta({ tipo: 'ok', texto }), [])
  const limpiarAlerta = useCallback(() => setAlerta(null), [])

  useEffect(() => {
    if (!alerta) return undefined
    const tiempo = ALERT_TIMEOUTS[alerta.tipo] ?? 2000
    const temporizador = setTimeout(() => {
      setAlerta((actual) => (actual === alerta ? null : actual))
    }, tiempo)

    return () => clearTimeout(temporizador)
  }, [alerta])

  if (!sesion) {
    return (
      <InicioSesion
        api={api}
        onLogin={guardarSesion}
        onError={mostrarError}
        onClearAlert={limpiarAlerta}
        onOk={mostrarOk}
        alerta={alerta}
      />
    )
  }

  return (
    <main className="app-shell">
      <header className="topbar">
        <div>
          <p className="eyebrow">SuEnvioSeguro S.A.</p>
          <h1>Operacion logistica</h1>
        </div>
        <div className="session-box">
          <span>{sesion.nombre}</span>
          <strong>{sesion.rol}</strong>
          <button type="button" className="secondary" onClick={cerrarSesion}>Salir</button>
        </div>
      </header>

      {alerta && <div className={`alert ${alerta.tipo}`}>{alerta.texto}</div>}

      <nav className="tabs" aria-label="Modulos principales">
        <button type="button" className={vista === 'facturacion' ? 'active' : ''} onClick={() => setVista('facturacion')}>Facturacion</button>
        <button type="button" className={vista === 'seguimiento' ? 'active' : ''} onClick={() => setVista('seguimiento')}>Seguimiento</button>
        {sesion.rol === 'ADMIN' && (
          <button type="button" className={vista === 'empleados' ? 'active' : ''} onClick={() => setVista('empleados')}>Empleados</button>
        )}
      </nav>

      {vista === 'facturacion' && <Facturacion api={api} onError={mostrarError} onOk={mostrarOk} />}
      {vista === 'seguimiento' && <Seguimiento api={api} onError={mostrarError} onOk={mostrarOk} />}
      {vista === 'empleados' && sesion.rol === 'ADMIN' && <Empleados api={api} onError={mostrarError} onOk={mostrarOk} />}
    </main>
  )
}

function InicioSesion({ api, onLogin, onError, onClearAlert, onOk, alerta }) {
  const [login, setLogin] = useState({ nombreUsuario: '', contrasena: '' })
  const [trackingCodigo, setTrackingCodigo] = useState('')
  const [tracking, setTracking] = useState(null)

  const ingresar = async (event) => {
    event.preventDefault()
    onClearAlert()
    try {
      const { data } = await api.post('/Auth/login', login)
      onLogin(data)
    } catch (error) {
      onError(error)
    }
  }

  const consultarTracking = async (event) => {
    event.preventDefault()
    try {
      const { data } = await api.get(`/Tracking/${trackingCodigo}`)
      setTracking(data)
    } catch (error) {
      setTracking(null)
      onError(error)
    }
  }

  return (
    <main className="login-layout">
      <section className="login-panel">
        <div className="login-header">
          <div className="logo-badge">📦</div>
          <p className="eyebrow">SuEnvioSeguro S.A.</p>
          <h1>Control de envíos y facturación</h1>
          <p className="copy">Ingreso para administradores y empleados. Los clientes consultan el estado con el código de envío.</p>
        </div>

        {alerta && <div className={`alert ${alerta.tipo}`}>{alerta.texto}</div>}

        <form className="form-grid login-form" onSubmit={ingresar}>
          <div className="form-group">
            <label htmlFor="usuario">👤 Usuario</label>
            <input 
              id="usuario"
              type="text"
              placeholder="Ingresa tu usuario"
              value={login.nombreUsuario} 
              onChange={(e) => setLogin({ ...login, nombreUsuario: e.target.value })} 
              required 
            />
          </div>
          <div className="form-group">
            <label htmlFor="contrasena">🔐 Contraseña</label>
            <input 
              id="contrasena"
              type="password"
              placeholder="Ingresa tu contraseña"
              value={login.contrasena} 
              onChange={(e) => setLogin({ ...login, contrasena: e.target.value })} 
              required 
            />
          </div>
          <button type="submit" className="btn-primary">Iniciar sesión</button>
        </form>
      </section>

      <section className="side-panel">
        <div className="tracking-header">
          <h2>📍 Consulta de envíos</h2>
          <p className="subtitle">Seguimiento público sin necesidad de credenciales</p>
        </div>
        <form className="inline-form tracking-form" onSubmit={consultarTracking}>
          <input 
            type="text"
            placeholder="Código de envío" 
            value={trackingCodigo} 
            onChange={(e) => setTrackingCodigo(e.target.value)} 
            required 
          />
          <button type="submit" className="secondary">Buscar</button>
        </form>
        {tracking && (
          <div className="tracking-result">
            <ResumenTracking tracking={tracking} />
          </div>
        )}
        {!tracking && trackingCodigo && (
          <div className="empty-state">
            <p>Ingresa un código de envío para consultar</p>
          </div>
        )}
      </section>
    </main>
  )
}

function Facturacion({ api, onError, onOk }) {
  const [formulario, setFormulario] = useState(envioVacio)
  const [resultado, setResultado] = useState(null)

  const actualizar = (campo, valor) => setFormulario({ ...formulario, [campo]: valor })

  const crearEnvio = async (event) => {
    event.preventDefault()
    try {
      const { data } = await api.post('/Envio/crear', {
        ...formulario,
        peso: Number(formulario.peso),
        cantidad: Number(formulario.cantidad),
      })
      setResultado(data)
      onOk('Envio registrado y factura generada.')
      setFormulario(envioVacio)
    } catch (error) {
      onError(error)
    }
  }

  return (
    <section className="workspace two-columns">
      <form className="module form-grid" onSubmit={crearEnvio}>
        <div className="module-header">
          <h2>Registrar envio</h2>
          <span>RF-03 / RF-05</span>
        </div>
        <Campo label="Documento cliente" value={formulario.clienteDocumento} onChange={(valor) => actualizar('clienteDocumento', valor)} required />
        <Campo label="Nombre cliente" value={formulario.clienteNombre} onChange={(valor) => actualizar('clienteNombre', valor)} required />
        <Campo label="Correo cliente" type="email" value={formulario.clienteCorreo} onChange={(valor) => actualizar('clienteCorreo', valor)} required />
        <Campo label="Telefono cliente" value={formulario.clienteTelefono} onChange={(valor) => actualizar('clienteTelefono', valor)} required />
        <Campo label="Direccion cliente" value={formulario.clienteDireccion} onChange={(valor) => actualizar('clienteDireccion', valor)} required />
        <Campo label="Descripcion" value={formulario.descripcion} onChange={(valor) => actualizar('descripcion', valor)} required />
        <label>
          Municipio destino
          <select value={formulario.municipioDestino} onChange={(event) => actualizar('municipioDestino', event.target.value)}>
            {municipios.map((municipio) => <option key={municipio}>{municipio}</option>)}
          </select>
        </label>
        <Campo label="Peso kg" type="number" min="1" max="30" value={formulario.peso} onChange={(valor) => actualizar('peso', valor)} required />
        <Campo label="Cantidad" type="number" min="1" value={formulario.cantidad} onChange={(valor) => actualizar('cantidad', valor)} required />
        <Campo label="Direccion entrega" value={formulario.direccion} onChange={(valor) => actualizar('direccion', valor)} required />
        <label className="check-row">
          <input type="checkbox" checked={formulario.esDelicado} onChange={(event) => actualizar('esDelicado', event.target.checked)} />
          Paquete delicado
        </label>
        <button type="submit">Generar factura</button>
      </form>

      <aside className="module summary-panel">
        <div className="module-header">
          <h2>Factura</h2>
          <span>IVA 19%</span>
        </div>
        {resultado ? (
          <ReciboFactura resultado={resultado} />
        ) : (
          <p className="muted">La factura generada aparecera aqui.</p>
        )}
      </aside>
    </section>
  )
}

function Seguimiento({ api, onError, onOk }) {
  const [codigo, setCodigo] = useState('')
  const [filtro, setFiltro] = useState({ estado: '', municipio: '', codigo: '' })
  const [tracking, setTracking] = useState(null)
  const [envios, setEnvios] = useState([])
  const [estadoNuevo, setEstadoNuevo] = useState('ENVIADO')
  const [paginaActual, setPaginaActual] = useState(1)

  const totalPaginas = Math.max(1, Math.ceil(envios.length / TRACKING_PAGE_SIZE))
  const inicioPagina = (paginaActual - 1) * TRACKING_PAGE_SIZE
  const finPagina = inicioPagina + TRACKING_PAGE_SIZE
  const enviosPaginados = useMemo(() => envios.slice(inicioPagina, finPagina), [envios, inicioPagina, finPagina])

  useEffect(() => {
    if (paginaActual > totalPaginas) {
      setPaginaActual(totalPaginas)
    }
  }, [paginaActual, totalPaginas])

  const buscarPublico = async (event) => {
    event.preventDefault()
    try {
      const { data } = await api.get(`/Tracking/${codigo}`)
      setTracking(data)
    } catch (error) {
      setTracking(null)
      onError(error)
    }
  }

  const cargarEnvios = async () => {
    try {
      const { data } = await api.get('/Envio', { params: filtro })
      setEnvios(data)
      setPaginaActual(1)
    } catch (error) {
      onError(error)
    }
  }

  const actualizarEstado = async () => {
    if (!codigo) return
    try {
      await api.put(`/Envio/actualizar-estado/${codigo}`, { nuevoEstado: estadoNuevo })
      onOk('Estado actualizado.')
      await cargarEnvios()
      const { data } = await api.get(`/Tracking/${codigo}`)
      setTracking(data)
    } catch (error) {
      onError(error)
    }
  }

  useEffect(() => {
    let activo = true
    const cargarInicial = async () => {
      try {
        const { data } = await api.get('/Envio')
        if (activo) setEnvios(data)
      } catch (error) {
        if (activo) onError(error)
      }
    }
    cargarInicial()
    return () => { activo = false }
  }, [api, onError])

  return (
    <section className="workspace">
      <div className="module">
        <div className="module-header">
          <h2>Monitor de envios</h2>
          <span>RF-04 / RF-06</span>
        </div>
        <form className="toolbar" onSubmit={(event) => { event.preventDefault(); cargarEnvios() }}>
          <input placeholder="Codigo" value={filtro.codigo} onChange={(event) => setFiltro({ ...filtro, codigo: event.target.value })} />
          <select value={filtro.estado} onChange={(event) => setFiltro({ ...filtro, estado: event.target.value })}>
            <option value="">Todos los estados</option>
            <option>REGISTRADO</option>
            <option>ENVIADO</option>
            <option>CANCELADO</option>
            <option>ENTREGADO</option>
          </select>
          <select value={filtro.municipio} onChange={(event) => setFiltro({ ...filtro, municipio: event.target.value })}>
            <option value="">Todos los municipios</option>
            {municipios.map((municipio) => <option key={municipio}>{municipio}</option>)}
          </select>
          <button type="submit" className="secondary">Filtrar</button>
        </form>
        <div className="table-wrap">
          <table>
            <thead>
              <tr>
                <th>Codigo</th>
                <th>Cliente</th>
                <th>Destino</th>
                <th>Estado</th>
                <th>Total</th>
              </tr>
            </thead>
            <tbody>
              {enviosPaginados.map((envio) => (
                <tr key={envio.codigoEnvio} onClick={() => setCodigo(envio.codigoEnvio)}>
                  <td>{envio.codigoEnvio}</td>
                  <td>{envio.cliente}</td>
                  <td>{envio.municipioDestino}</td>
                  <td><span className="status">{envio.estado}</span></td>
                  <td>{formatoMoneda(envio.totalAPagar)}</td>
                </tr>
              ))}
              {!enviosPaginados.length && (
                <tr>
                  <td colSpan="5" className="muted">No hay envios para mostrar.</td>
                </tr>
              )}
            </tbody>
          </table>
        </div>
        <div className="pagination">
          <p className="pagination-info">
            Mostrando {envios.length ? inicioPagina + 1 : 0}-{Math.min(finPagina, envios.length)} de {envios.length}
          </p>
          <div className="pagination-actions">
            <button type="button" className="secondary" onClick={() => setPaginaActual((actual) => Math.max(1, actual - 1))} disabled={paginaActual === 1}>
              Anterior
            </button>
            <span>Pagina {paginaActual} de {totalPaginas}</span>
            <button type="button" className="secondary" onClick={() => setPaginaActual((actual) => Math.min(totalPaginas, actual + 1))} disabled={paginaActual === totalPaginas}>
              Siguiente
            </button>
          </div>
        </div>
      </div>

      <div className="module two-columns narrow">
        <form className="form-grid" onSubmit={buscarPublico}>
          <div className="module-header">
            <h2>Consultar codigo</h2>
            <span>RU-04</span>
          </div>
          <Campo label="Codigo envio" value={codigo} onChange={setCodigo} required />
          <label>
            Nuevo estado
            <select value={estadoNuevo} onChange={(event) => setEstadoNuevo(event.target.value)}>
              {estados.map((estado) => <option key={estado}>{estado}</option>)}
            </select>
          </label>
          <button type="submit">Consultar</button>
          <button type="button" className="secondary" onClick={actualizarEstado}>Actualizar estado</button>
        </form>
        <div>
          {tracking ? <ResumenTracking tracking={tracking} /> : <p className="muted">Selecciona o digita un codigo de envio.</p>}
        </div>
      </div>
    </section>
  )
}

function Empleados({ api, onError, onOk }) {
  const [empleados, setEmpleados] = useState([])
  const [formulario, setFormulario] = useState(empleadoVacio)
  const [editandoId, setEditandoId] = useState(null)

  const cargarEmpleados = async () => {
    try {
      const { data } = await api.get('/Empleados')
      setEmpleados(data)
    } catch (error) {
      onError(error)
    }
  }

  useEffect(() => {
    let activo = true
    const cargarInicial = async () => {
      try {
        const { data } = await api.get('/Empleados')
        if (activo) setEmpleados(data)
      } catch (error) {
        if (activo) onError(error)
      }
    }
    cargarInicial()
    return () => { activo = false }
  }, [api, onError])

  const guardar = async (event) => {
    event.preventDefault()
    try {
      if (editandoId) {
        const payload = { ...formulario }
        if (!payload.contrasena) delete payload.contrasena
        await api.put(`/Empleados/${editandoId}`, payload)
        onOk('Empleado actualizado.')
      } else {
        await api.post('/Empleados', formulario)
        onOk('Empleado creado.')
      }
      setFormulario(empleadoVacio)
      setEditandoId(null)
      await cargarEmpleados()
    } catch (error) {
      onError(error)
    }
  }

  const editar = (empleado) => {
    setEditandoId(empleado.id)
    setFormulario({ ...empleado, contrasena: '' })
  }

  const desactivar = async (empleado) => {
    try {
      await api.delete(`/Empleados/${empleado.id}`)
      onOk('Empleado desactivado.')
      await cargarEmpleados()
    } catch (error) {
      onError(error)
    }
  }

  const activar = async (empleado) => {
    try {
      await api.patch(`/Empleados/${empleado.id}/activar`)
      onOk('Empleado activado.')
      await cargarEmpleados()
    } catch (error) {
      onError(error)
    }
  }

  return (
    <section className="workspace two-columns">
      <form className="module form-grid" onSubmit={guardar}>
        <div className="module-header">
          <h2>{editandoId ? 'Editar empleado' : 'Nuevo empleado'}</h2>
          <span>ABC-5</span>
        </div>
        <Campo label="Documento" value={formulario.documentoIdentidad} onChange={(valor) => setFormulario({ ...formulario, documentoIdentidad: valor })} required />
        <Campo label="Usuario" value={formulario.nombreUsuario} onChange={(valor) => setFormulario({ ...formulario, nombreUsuario: valor })} required />
        <Campo label="Nombre" value={formulario.nombre} onChange={(valor) => setFormulario({ ...formulario, nombre: valor })} required />
        <Campo label="Correo" type="email" value={formulario.correo} onChange={(valor) => setFormulario({ ...formulario, correo: valor })} required />
        <Campo label="Telefono" value={formulario.telefono} onChange={(valor) => setFormulario({ ...formulario, telefono: valor })} />
        <Campo label="Direccion" value={formulario.direccion} onChange={(valor) => setFormulario({ ...formulario, direccion: valor })} />
        <Campo label={editandoId ? 'Nueva contrasena' : 'Contrasena'} type="password" value={formulario.contrasena || ''} onChange={(valor) => setFormulario({ ...formulario, contrasena: valor })} required={!editandoId} />
        <label>
          Rol
          <select value={formulario.rol} onChange={(event) => setFormulario({ ...formulario, rol: event.target.value })}>
            <option>EMPLEADO</option>
            <option>ADMIN</option>
          </select>
        </label>
        {editandoId && (
          <label className="check-row">
            <input type="checkbox" checked={formulario.activo} onChange={(event) => setFormulario({ ...formulario, activo: event.target.checked })} />
            Activo
          </label>
        )}
        <button type="submit">Guardar empleado</button>
        {editandoId && <button type="button" className="secondary" onClick={() => { setFormulario(empleadoVacio); setEditandoId(null) }}>Cancelar</button>}
      </form>

      <div className="module">
        <div className="module-header">
          <h2>Equipo</h2>
          <span>{empleados.length} registros</span>
        </div>
        <div className="employee-list">
          {empleados.map((empleado) => (
            <article key={empleado.id} className="employee-row">
              <div>
                <strong>{empleado.nombre}</strong>
                <span>{empleado.nombreUsuario} - {empleado.rol}</span>
              </div>
              <span className={empleado.activo ? 'badge ok' : 'badge off'}>{empleado.activo ? 'Activo' : 'Inactivo'}</span>
              <button type="button" className="secondary" onClick={() => editar(empleado)}>Editar</button>
              {empleado.activo
                ? <button type="button" className="danger" onClick={() => desactivar(empleado)}>Desactivar</button>
                : <button type="button" className="secondary" onClick={() => activar(empleado)}>Activar</button>}
            </article>
          ))}
        </div>
      </div>
    </section>
  )
}

function Campo({ label, value, onChange, type = 'text', required = false, ...props }) {
  return (
    <label>
      {label}
      <input type={type} value={value} onChange={(event) => onChange(event.target.value)} required={required} {...props} />
    </label>
  )
}

function ResumenTracking({ tracking }) {
  return (
    <dl className="tracking-card">
      <div><dt>Envio</dt><dd>{tracking.codigoEnvio}</dd></div>
      <div><dt>Factura</dt><dd>{tracking.codigoFactura}</dd></div>
      <div><dt>Destino</dt><dd>{tracking.destino}</dd></div>
      <div><dt>Direccion</dt><dd>{tracking.direccionEntrega}</dd></div>
      <div><dt>Estado</dt><dd><span className="status">{tracking.estado}</span></dd></div>
      <div><dt>Total</dt><dd>{formatoMoneda(tracking.valorTotal)}</dd></div>
    </dl>
  )
}

function ReciboFactura({ resultado }) {
  const fechaActual = new Date().toLocaleDateString('es-CO', {
    year: 'numeric',
    month: 'long',
    day: 'numeric'
  })

  const montoBase = resultado.totalAPagar / 1.19
  const iva = resultado.totalAPagar - montoBase

  const imprimirFactura = () => {
    const html = `<!DOCTYPE html>
<html lang="es">
<head>
  <meta charset="UTF-8">
  <meta name="viewport" content="width=device-width, initial-scale=1.0">
  <title>Factura ${resultado.codigoFactura}</title>
  <style>
    * { margin: 0; padding: 0; box-sizing: border-box; }
    body { font-family: -apple-system, BlinkMacSystemFont, Segoe UI, Roboto, sans-serif; background: white; color: #000; padding: 40px; line-height: 1.6; }
    .invoice-wrapper { max-width: 800px; margin: 0 auto; }
    .invoice-header { display: grid; grid-template-columns: auto 1fr auto; gap: 24px; align-items: center; margin-bottom: 30px; padding-bottom: 20px; border-bottom: 2px solid #000; }
    .invoice-logo { font-size: 40px; border: 2px solid #000; padding: 10px; border-radius: 4px; width: 60px; height: 60px; display: flex; align-items: center; justify-content: center; }
    .invoice-company h3 { font-size: 18px; font-weight: bold; margin-bottom: 4px; }
    .invoice-company p { font-size: 12px; margin: 2px 0; color: #333; }
    .invoice-date { text-align: right; }
    .invoice-date p { font-size: 12px; margin: 4px 0; }
    .invoice-date strong { font-weight: bold; }
    .invoice-details { margin: 30px 0; }
    .detail-section h4 { font-size: 13px; font-weight: bold; text-transform: uppercase; letter-spacing: 0.5px; margin-bottom: 12px; border-bottom: 1px solid #000; padding-bottom: 8px; }
    .detail-section dl { display: grid; gap: 8px; }
    .detail-section div { display: grid; grid-template-columns: 140px 1fr; gap: 12px; }
    .detail-section dt { font-weight: bold; font-size: 12px; }
    .detail-section dd { margin: 0; font-size: 12px; }
    .invoice-totals { border: 1px solid #000; padding: 16px; margin: 30px 0; }
    .total-row { display: flex; justify-content: space-between; padding: 8px 0; border-bottom: 1px solid #ccc; font-size: 12px; }
    .total-row:last-child { border-bottom: none; }
    .total-row span:first-child { font-weight: bold; }
    .total-row span:last-child { font-weight: bold; }
    .total-row.total-amount { padding: 12px 0; border-top: 2px solid #000; border-bottom: none; margin-top: 8px; padding-top: 8px; font-size: 14px; font-weight: bold; }
    .invoice-footer { text-align: center; padding-top: 20px; border-top: 1px solid #000; margin-top: 30px; font-size: 11px; }
    .invoice-footer p { margin: 4px 0; }
    @media print { body { padding: 20px; } .invoice-wrapper { max-width: 100%; } }
  </style>
</head>
<body>
  <div class="invoice-wrapper">
    <div class="invoice-header">
      <div class="invoice-logo">📦</div>
      <div class="invoice-company">
        <h3>SuEnvioSeguro S.A.</h3>
        <p>Servicios de Envío y Logística</p>
        <p>NIT: 123456789-0</p>
      </div>
      <div class="invoice-date">
        <p><strong>Fecha:</strong> ${fechaActual}</p>
        <p><strong>Factura:</strong> ${resultado.codigoFactura}</p>
      </div>
    </div>
    <div class="invoice-details">
      <div class="detail-section">
        <h4>Datos del Envío</h4>
        <dl>
          <div><dt>Código Envío:</dt><dd>${resultado.codigoEnvio}</dd></div>
          <div><dt>Estado:</dt><dd>${resultado.estado}</dd></div>
          <div><dt>Destino:</dt><dd>${resultado.municipioDestino || resultado.destino}</dd></div>
          <div><dt>Descripción:</dt><dd>${resultado.descripcion || 'N/A'}</dd></div>
        </dl>
      </div>
    </div>
    <div class="invoice-totals">
      <div class="total-row">
        <span>Subtotal:</span>
        <span>${formatoMoneda(montoBase)}</span>
      </div>
      <div class="total-row">
        <span>IVA (19%):</span>
        <span>${formatoMoneda(iva)}</span>
      </div>
      <div class="total-row total-amount">
        <span>Total a Pagar:</span>
        <span>${formatoMoneda(resultado.totalAPagar)}</span>
      </div>
    </div>
    <div class="invoice-footer">
      <p>Gracias por usar SuEnvioSeguro</p>
      <p>Para consultar el estado: www.suenvioseguro.com</p>
    </div>
  </div>
  <script>
    window.print();
    window.onafterprint = function() { window.close(); };
  <\/script>
</body>
</html>`
    const ventana = window.open('', 'factura', 'width=850,height=600')
    ventana.document.write(html)
    ventana.document.close()
  }

  return (
    <div className="invoice-container print-only-container">
      <div className="invoice-header">
        <div className="invoice-logo">📦</div>
        <div className="invoice-company">
          <h3>SuEnvioSeguro S.A.</h3>
          <p>Servicios de Envío y Logística</p>
          <p>NIT: 123456789-0</p>
        </div>
        <div className="invoice-date">
          <p><strong>Fecha:</strong> {fechaActual}</p>
          <p><strong>Factura:</strong> {resultado.codigoFactura}</p>
        </div>
      </div>

      <div className="invoice-divider"></div>

      <div className="invoice-details">
        <div className="detail-section">
          <h4>Datos del Envío</h4>
          <dl>
            <div><dt>Código Envío:</dt><dd>{resultado.codigoEnvio}</dd></div>
            <div><dt>Estado:</dt><dd>{resultado.estado}</dd></div>
            <div><dt>Destino:</dt><dd>{resultado.municipioDestino || resultado.destino || 'N/A'}</dd></div>
            <div><dt>Descripción:</dt><dd>{resultado.descripcion || 'N/A'}</dd></div>
          </dl>
        </div>
      </div>

      <div className="invoice-divider"></div>

      <div className="invoice-totals">
        <div className="total-row">
          <span>Subtotal:</span>
          <span>{formatoMoneda(montoBase)}</span>
        </div>
        <div className="total-row">
          <span>IVA (19%):</span>
          <span>{formatoMoneda(iva)}</span>
        </div>
        <div className="total-row total-amount">
          <span>Total a Pagar:</span>
          <span>{formatoMoneda(resultado.totalAPagar)}</span>
        </div>
      </div>

      <div className="invoice-footer">
        <p>Gracias por usar SuEnvioSeguro</p>
        <p>Para consultar el estado: www.suenvioseguro.com</p>
      </div>

      <button type="button" className="btn-print" onClick={imprimirFactura}>
        🖨️ Imprimir Factura
      </button>
    </div>
  )
}

function formatoMoneda(valor) {
  return new Intl.NumberFormat('es-CO', {
    style: 'currency',
    currency: 'COP',
    maximumFractionDigits: 0,
  }).format(Number(valor || 0))
}

export default App