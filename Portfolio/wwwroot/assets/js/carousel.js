// Project image carousel (scroll-snap based) + lightbox modal.
// Markup: figure[data-gallery] > .gallery-track > button.gallery-slide[data-full] > img
const prefersReducedMotion = window.matchMedia("(prefers-reduced-motion: reduce)").matches;

const icons = {
    prev: '<svg viewBox="0 0 24 24" aria-hidden="true"><path d="m15 18-6-6 6-6"/></svg>',
    next: '<svg viewBox="0 0 24 24" aria-hidden="true"><path d="m9 18 6-6-6-6"/></svg>',
    close: '<svg viewBox="0 0 24 24" aria-hidden="true"><path d="M18 6 6 18M6 6l12 12"/></svg>',
};

const createButton = (className, label, icon) => {
    const button = document.createElement("button");
    button.type = "button";
    button.className = className;
    button.setAttribute("aria-label", label);
    button.innerHTML = icon;
    return button;
};

// ---- Lightbox -------------------------------------------------------------
let lightbox = null;

const getLightbox = () => {
    if (lightbox) {
        return lightbox;
    }

    const dialog = document.createElement("dialog");
    dialog.className = "lightbox";
    dialog.setAttribute("aria-label", "Afbeelding vergroot");

    const image = document.createElement("img");
    image.className = "lightbox-image";
    image.alt = "";

    const counter = document.createElement("p");
    counter.className = "lightbox-counter";

    const closeButton = createButton("lightbox-button lightbox-close", "Sluiten", icons.close);
    const prevButton = createButton("lightbox-button lightbox-prev", "Vorige afbeelding", icons.prev);
    const nextButton = createButton("lightbox-button lightbox-next", "Volgende afbeelding", icons.next);

    dialog.append(image, counter, closeButton, prevButton, nextButton);
    document.body.appendChild(dialog);

    const state = { slides: [], index: 0, trigger: null };

    const show = (index) => {
        const total = state.slides.length;
        state.index = (index + total) % total;

        const slide = state.slides[state.index];
        image.src = slide.full;
        image.alt = slide.alt;
        counter.textContent = total > 1 ? `${state.index + 1} / ${total}` : "";
        dialog.classList.toggle("is-single", total <= 1);
    };

    closeButton.addEventListener("click", () => dialog.close());
    prevButton.addEventListener("click", () => show(state.index - 1));
    nextButton.addEventListener("click", () => show(state.index + 1));

    // Click on the dark backdrop (outside the image and buttons) closes the dialog
    dialog.addEventListener("click", (event) => {
        if (event.target === dialog) {
            dialog.close();
        }
    });

    dialog.addEventListener("keydown", (event) => {
        if (event.key === "ArrowLeft") {
            show(state.index - 1);
        } else if (event.key === "ArrowRight") {
            show(state.index + 1);
        }
    });

    dialog.addEventListener("close", () => {
        document.body.classList.remove("has-lightbox");
        image.removeAttribute("src");
        state.trigger?.focus({ preventScroll: true });
    });

    lightbox = {
        open(slides, index, trigger) {
            state.slides = slides;
            state.trigger = trigger;
            show(index);
            document.body.classList.add("has-lightbox");
            dialog.showModal();
        },
    };

    return lightbox;
};

// ---- Carousel -------------------------------------------------------------
const initGallery = (gallery) => {
    const track = gallery.querySelector(".gallery-track");
    const slideElements = Array.from(gallery.querySelectorAll(".gallery-slide"));

    if (!track || slideElements.length === 0) {
        return;
    }

    const slides = slideElements.map((element) => ({
        full: element.dataset.full,
        alt: element.querySelector("img")?.alt ?? "",
    }));

    slideElements.forEach((element, index) => {
        element.addEventListener("click", () => getLightbox().open(slides, index, element));
    });

    if (slideElements.length < 2) {
        return;
    }

    const dots = document.createElement("div");
    dots.className = "gallery-dots";

    const goTo = (index) => {
        const target = (index + slideElements.length) % slideElements.length;
        track.scrollTo({
            left: target * track.clientWidth,
            behavior: prefersReducedMotion ? "auto" : "smooth",
        });
    };

    const dotElements = slideElements.map((_, index) => {
        const dot = document.createElement("button");
        dot.type = "button";
        dot.className = "gallery-dot";
        dot.setAttribute("aria-label", `Afbeelding ${index + 1}`);
        dot.addEventListener("click", () => goTo(index));
        dots.appendChild(dot);
        return dot;
    });

    const prevButton = createButton("gallery-button gallery-prev", "Vorige afbeelding", icons.prev);
    const nextButton = createButton("gallery-button gallery-next", "Volgende afbeelding", icons.next);

    gallery.append(prevButton, nextButton, dots);

    const currentIndex = () => Math.round(track.scrollLeft / track.clientWidth);

    const sync = () => {
        const index = currentIndex();
        dotElements.forEach((dot, dotIndex) => {
            dot.classList.toggle("is-active", dotIndex === index);
            dot.setAttribute("aria-current", String(dotIndex === index));
        });
    };

    prevButton.addEventListener("click", () => goTo(currentIndex() - 1));
    nextButton.addEventListener("click", () => goTo(currentIndex() + 1));
    track.addEventListener("scroll", sync, { passive: true });
    window.addEventListener("resize", () => track.scrollTo({ left: currentIndex() * track.clientWidth }));
    sync();
};

document.querySelectorAll("[data-gallery]").forEach(initGallery);
