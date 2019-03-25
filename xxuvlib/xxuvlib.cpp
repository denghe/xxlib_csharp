#include "xxuvlib.h"
#include <stdlib.h>
#include <string.h>
#include <stdio.h>

#ifdef _WIN32

BOOL APIENTRY DllMain(HMODULE hModule,
	DWORD  ul_reason_for_call,
	LPVOID lpReserved
)
{
	switch (ul_reason_for_call)
	{
	case DLL_PROCESS_ATTACH:
	case DLL_THREAD_ATTACH:
	case DLL_THREAD_DETACH:
	case DLL_PROCESS_DETACH:
		break;
	}
	return TRUE;
}
#else
#define sprintf_s snprintf
#endif


// 分配内存时保留一个头空间并填充
static void* Alloc(size_t size, void* ud = nullptr)
{
	auto p = (void**)malloc(size + sizeof(void*));
	p[0] = ud;
	return &p[1];
}

// 还原真实指针, 释放
static void Free(void* p) noexcept
{
	free((void**)p - 1);
}

static void AllocCB(uv_handle_t* h, size_t suggested_size, uv_buf_t* buf)
{
	buf->base = (char*)Alloc(suggested_size);
	buf->len = decltype(buf->len)(suggested_size);
}




XXUVLIB_API uv_loop_t* xxuv_alloc_uv_loop_t(void* ud) noexcept
{
	return (uv_loop_t*)Alloc(sizeof(uv_loop_t), ud);
}

XXUVLIB_API uv_tcp_t* xxuv_alloc_uv_tcp_t(void* ud) noexcept
{
	return (uv_tcp_t*)Alloc(sizeof(uv_tcp_t), ud);
}

XXUVLIB_API uv_udp_t* xxuv_alloc_uv_udp_t(void* ud) noexcept
{
	return (uv_udp_t*)Alloc(sizeof(uv_udp_t), ud);
}


XXUVLIB_API sockaddr* xxuv_alloc_sockaddr_in(void* ud) noexcept
{
	auto ptr = Alloc(sizeof(sockaddr_in6), ud);
	memset(ptr, 0, sizeof(sockaddr_in6));
	return (sockaddr*)ptr;
}

XXUVLIB_API uv_timer_t* xxuv_alloc_uv_timer_t(void* ud) noexcept
{
	return (uv_timer_t*)Alloc(sizeof(uv_timer_t), ud);
}

XXUVLIB_API uv_async_t* xxuv_alloc_uv_async_t(void* ud) noexcept
{
	return (uv_async_t*)Alloc(sizeof(uv_async_t), ud);
}

XXUVLIB_API uv_signal_t* xxuv_alloc_uv_signal_t(void* ud) noexcept
{
	return (uv_signal_t*)Alloc(sizeof(uv_signal_t), ud);
}





XXUVLIB_API void xxuv_free(void* p) noexcept
{
	if (p) Free(p);
}

XXUVLIB_API void* xxuv_get_ud(void* p) noexcept
{
	return *((void**)p - 1);
}

XXUVLIB_API void* xxuv_get_ud_from_uv_connect_t(uv_connect_t* req) noexcept
{
	return xxuv_get_ud(req->handle);
}

XXUVLIB_API void* xxuv_get_buf(uv_buf_t* p) noexcept
{
	return p->base;
}





XXUVLIB_API const char* xxuv_strerror(int n) noexcept
{
	return uv_strerror(n);
}
XXUVLIB_API const char* xxuv_err_name(int n) noexcept
{
	return uv_err_name(n);
}







XXUVLIB_API void xxuv_close(uv_handle_t* handle, uv_close_cb close_cb) noexcept
{
	uv_close(handle, close_cb);
}

XXUVLIB_API void xxuv_close_(uv_handle_t* handle) noexcept
{
#ifndef NDEBUG
	if (uv_is_closing(handle)) return;
#endif
	uv_close(handle, [](uv_handle_t* handle)
	{
		Free(handle);
	});
}





XXUVLIB_API int xxuv_loop_init(uv_loop_t* p) noexcept
{
	return uv_loop_init(p);
}

XXUVLIB_API int xxuv_run(uv_loop_t* loop, uv_run_mode mode) noexcept
{
	return uv_run(loop, mode);
}

XXUVLIB_API void xxuv_stop(uv_loop_t* loop) noexcept
{
	uv_stop(loop);
}

XXUVLIB_API int xxuv_loop_close(uv_loop_t* p) noexcept
{
	return uv_loop_close(p);
}

XXUVLIB_API int xxuv_loop_alive(uv_loop_t* p) noexcept
{
	return uv_loop_alive(p);
}



XXUVLIB_API int xxuv_ip4_addr(const char* ipv4, int port, sockaddr* addr) noexcept
{
	return uv_ip4_addr(ipv4, port, (sockaddr_in*)addr);
}
XXUVLIB_API int xxuv_ip6_addr(const char* ipv6, int port, sockaddr_in6* addr) noexcept
{
	return uv_ip6_addr(ipv6, port, addr);
}



XXUVLIB_API int xxuv_tcp_init(uv_loop_t* loop, uv_tcp_t* tcp) noexcept
{
	return uv_tcp_init(loop, tcp);
}

XXUVLIB_API int xxuv_tcp_bind(uv_tcp_t* tcp, const sockaddr* addr, unsigned int flags) noexcept
{
	return uv_tcp_bind(tcp, addr, flags);
}
XXUVLIB_API int xxuv_tcp_bind_(uv_tcp_t* tcp, const sockaddr* addr) noexcept
{
	return uv_tcp_bind(tcp, addr, 0);
}


XXUVLIB_API int xxuv_listen(uv_stream_t* listener, int backlog, uv_connection_cb cb) noexcept
{
	return uv_listen(listener, backlog, cb);
}

XXUVLIB_API int xxuv_accept(uv_stream_t* listener, uv_stream_t* peer) noexcept
{
	return uv_accept(listener, peer);
}

XXUVLIB_API int xxuv_read_start(uv_stream_t* stream, uv_alloc_cb alloc_cb, uv_read_cb read_cb) noexcept
{
	return uv_read_start(stream, alloc_cb, read_cb);
}
XXUVLIB_API int xxuv_read_start_(uv_stream_t* stream, uv_read_cb read_cb) noexcept
{
	return uv_read_start(stream, AllocCB, read_cb);
}

XXUVLIB_API int xxuv_write(uv_write_t* req, uv_stream_t* stream, const uv_buf_t bufs[], unsigned int nbufs, uv_write_cb cb) noexcept
{
	return uv_write(req, stream, bufs, nbufs, cb);
}

XXUVLIB_API int xxuv_write_(uv_stream_t* stream, char const* inBuf, unsigned int offset, unsigned int len) noexcept
{
	struct uv_write_t_ex : uv_write_t
	{
		uv_buf_t buf;
	};
	auto req = (uv_write_t_ex*)Alloc(sizeof(uv_write_t_ex) + len);
	memcpy(req + 1, inBuf + offset, len);
	req->buf.base = (char*)(req + 1);
	req->buf.len = decltype(uv_buf_t::len)(len);
	return uv_write(req, stream, &req->buf, 1, [](uv_write_t *req, int status)
	{
		Free(req);
	});
}


XXUVLIB_API int xxuv_fill_client_ip(uv_tcp_t* stream, char* buf, int buf_len, int* data_len) noexcept
{
	sockaddr_in6 saddr;
	int len = sizeof(saddr);
	int r = 0;
	if (r = uv_tcp_getpeername(stream, (sockaddr*)&saddr, &len)) return r;
	if (((sockaddr*)&saddr)->sa_family == AF_INET6)
	{
		if (r = uv_inet_ntop(AF_INET6, &saddr.sin6_addr, buf, buf_len)) return r;
		*data_len = (int)strlen(buf);
		*data_len += sprintf_s(buf + *data_len, buf_len - *data_len, ":%d", ntohs(saddr.sin6_port));
	}
	else
	{
		if (r = uv_inet_ntop(AF_INET, &((sockaddr_in*)&saddr)->sin_addr, buf, buf_len)) return r;
		*data_len = (int)strlen(buf);
		*data_len += sprintf_s(buf + *data_len, buf_len - *data_len, ":%d", ntohs(((sockaddr_in*)&saddr)->sin_port));
	}
	return r;
}


XXUVLIB_API int xxuv_tcp_connect(uv_connect_t* req, uv_tcp_t* handle, const sockaddr* addr, uv_connect_cb cb) noexcept
{
	return uv_tcp_connect(req, handle, addr, cb);
}

XXUVLIB_API int xxuv_tcp_connect_(uv_tcp_t* handle, const struct sockaddr* addr, uv_connect_cb cb) noexcept
{
	auto req = (uv_connect_t*)Alloc(sizeof(uv_connect_t));
	auto r = uv_tcp_connect(req, handle, addr, cb);
	if (r) Free(req);
	return r;
}








XXUVLIB_API int xxuv_timer_init(uv_loop_t* loop, uv_timer_t* timer_req) noexcept
{
	return uv_timer_init(loop, timer_req);
}
XXUVLIB_API int xxuv_timer_start(uv_timer_t* timer_req, uv_timer_cb cb, unsigned long long timeoutMS, unsigned long long repeatIntervalMS) noexcept
{
	return uv_timer_start(timer_req, cb, timeoutMS, repeatIntervalMS);
}
XXUVLIB_API void xxuv_timer_set_repeat(uv_timer_t* timer_req, unsigned long long repeatIntervalMS) noexcept
{
	uv_timer_set_repeat(timer_req, repeatIntervalMS);
}
XXUVLIB_API int xxuv_timer_again(uv_timer_t* timer_req) noexcept
{
	return uv_timer_again(timer_req);
}
XXUVLIB_API int xxuv_timer_stop(uv_timer_t* timer_req) noexcept
{
	return uv_timer_stop(timer_req);
}





XXUVLIB_API int xxuv_async_init(uv_loop_t* loop, uv_async_t* async_req, uv_async_cb cb) noexcept
{
	return uv_async_init(loop, async_req, cb);
}
XXUVLIB_API int xxuv_async_send(uv_async_t* async_req) noexcept
{
	return uv_async_send(async_req);
}





XXUVLIB_API int xxuv_signal_init(uv_loop_t* loop, uv_signal_t* signal) noexcept
{
	return uv_signal_init(loop, signal);
}
XXUVLIB_API int xxuv_signal_start(uv_signal_t* signal, uv_signal_cb cb) noexcept
{
	return uv_signal_start(signal, cb, SIGINT);
}
XXUVLIB_API void xxuv_walk(uv_loop_t* loop, uv_walk_cb cb, void* arg) noexcept
{
	uv_walk(loop, cb, arg);
}







XXUVLIB_API int xxuv_is_readable(const uv_stream_t* stream) noexcept
{
	return uv_is_readable(stream);
}
XXUVLIB_API int xxuv_is_writable(const uv_stream_t* stream) noexcept
{
	return uv_is_writable(stream);
}
XXUVLIB_API size_t xxuv_stream_get_write_queue_size(const uv_stream_t* stream) noexcept
{
	return uv_stream_get_write_queue_size(stream);
}
XXUVLIB_API int xxuv_try_write(uv_stream_t* stream, const uv_buf_t bufs[], unsigned int nbufs) noexcept
{
	return uv_try_write(stream, bufs, nbufs);
}
XXUVLIB_API int xxuv_try_write_(uv_stream_t* stream, char* buf, unsigned int len) noexcept
{
	auto bufs = uv_buf_init(buf, len);
	return uv_try_write(stream, &bufs, 1);
}

