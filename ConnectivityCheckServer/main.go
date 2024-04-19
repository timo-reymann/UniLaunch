package main

import (
	"flag"
	"log"
	"net/http"
	"os"
	"strings"
	"time"
)

type connectivityHttpHandler struct {
	privacyUrl *string
}

var okResponse = []byte("okResponse")

func (h connectivityHttpHandler) ServeHTTP(w http.ResponseWriter, r *http.Request) {
	if h.privacyUrl != nil {
		accept := r.Header.Get("Accept")
		if accept == "" || !strings.Contains(accept, "html") {
			_, _ = w.Write(okResponse)
			return
		}
	}

	http.Redirect(w, r, *h.privacyUrl, http.StatusTemporaryRedirect)
}

func configVal(name string) *string {
	envVal := os.Getenv(strings.ToUpper("uc_" + strings.ReplaceAll(name, "-", "_")))
	if envVal != "" {
		return &envVal
	}

	flagVal := flag.String(name, "", "")
	return flagVal
}

func main() {
	handler := connectivityHttpHandler{
		privacyUrl: configVal("privacy-url"),
	}
	flag.Parse()

	s := &http.Server{
		Addr:           ":8080",
		Handler:        handler,
		ReadTimeout:    1 * time.Second,
		WriteTimeout:   1 * time.Second,
		MaxHeaderBytes: 1 << 20,
		IdleTimeout:    1 * time.Second,
	}
	log.Fatal(s.ListenAndServe())
}
