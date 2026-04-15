document.addEventListener("DOMContentLoaded", () => {
    const prefersReducedMotion = window.matchMedia("(prefers-reduced-motion: reduce)").matches;
    const headerShell = document.querySelector(".header-shell");
    const footerShell = document.querySelector(".footer-shell");
    const menuToggle = document.querySelector(".menu-toggle");
    const siteNav = document.querySelector("#site-nav");
    const scrollTopButton = document.querySelector(".scroll-top");
    const trackedSections = Array.from(document.querySelectorAll("main section[id]"));

    const typeText = (element, options = {}) => {
        if (!element) {
            return;
        }

        const fullText = element.dataset.text || element.textContent || "";
        const typingClass = options.typingClass || "is-typing";
        const completeClass = options.completeClass || "is-complete";
        const intervalMs = options.intervalMs || 55;

        element.textContent = "";

        if (prefersReducedMotion) {
            element.textContent = fullText;
            element.classList.add(completeClass);
            return;
        }

        element.classList.add(typingClass);

        let index = 0;
        const typeInterval = window.setInterval(() => {
            element.textContent += fullText.charAt(index);
            index += 1;

            if (index >= fullText.length) {
                window.clearInterval(typeInterval);
                element.classList.remove(typingClass);
                element.classList.add(completeClass);
            }
        }, intervalMs);
    };

    if (menuToggle && siteNav) {
        const closeMenu = () => {
            menuToggle.classList.remove("is-open");
            siteNav.classList.remove("is-open");
            menuToggle.setAttribute("aria-expanded", "false");
        };

        menuToggle.addEventListener("click", () => {
            const isOpen = menuToggle.classList.toggle("is-open");
            siteNav.classList.toggle("is-open", isOpen);
            menuToggle.setAttribute("aria-expanded", String(isOpen));
        });

        siteNav.querySelectorAll("a").forEach((link) => {
            link.addEventListener("click", closeMenu);
        });

        window.addEventListener("resize", () => {
            if (window.innerWidth >= 992) {
                closeMenu();
            }
        });
    }

    const heroName = document.querySelector(".hero-name");
    typeText(heroName);

    const brandText = document.querySelector(".brand-text");
    typeText(brandText, {
        typingClass: "brand-is-typing",
        completeClass: "brand-is-complete",
        intervalMs: 75,
    });

    const particleField = document.querySelector(".particle-field");
    if (scrollTopButton) {
        const updateScrollTopButton = () => {
            const scrollHeight = document.documentElement.scrollHeight - window.innerHeight;
            const scrollProgress = scrollHeight > 0 ? (window.scrollY / scrollHeight) * 100 : 0;
            const baseBottom = window.innerWidth <= 560 ? 18 : 34;
            const footerOffset = footerShell
                ? Math.max(0, window.innerHeight - footerShell.getBoundingClientRect().top + 28)
                : 0;

            scrollTopButton.style.setProperty("--progress", `${scrollProgress}`);
            scrollTopButton.style.bottom = `${baseBottom + footerOffset}px`;
            scrollTopButton.classList.toggle("is-visible", window.scrollY > 320);
        };

        scrollTopButton.addEventListener("click", () => {
            window.scrollTo({
                top: 0,
                behavior: prefersReducedMotion ? "auto" : "smooth",
            });
        });

        window.addEventListener("scroll", updateScrollTopButton, { passive: true });
        window.addEventListener("resize", updateScrollTopButton);
        updateScrollTopButton();
    }

    if (headerShell) {
        const updateHeaderState = () => {
            headerShell.classList.toggle("is-scrolled", window.scrollY > 16);
        };

        window.addEventListener("scroll", updateHeaderState, { passive: true });
        window.addEventListener("resize", updateHeaderState);
        updateHeaderState();
    }

    if (trackedSections.length > 0) {
        let activeHash = window.location.hash;

        const updateLocationHash = (sectionId) => {
            const nextHash = sectionId ? `#${sectionId}` : "";

            if (nextHash === activeHash) {
                return;
            }

            const nextUrl = nextHash || `${window.location.pathname}${window.location.search}`;
            window.history.replaceState(null, "", nextUrl);
            activeHash = nextHash;
        };

        const getCurrentSectionId = () => {
            const headerOffset = headerShell ? headerShell.offsetHeight : 0;
            const viewportMarker = Math.min(
                window.innerHeight - 1,
                headerOffset + Math.max(120, (window.innerHeight - headerOffset) * 0.35)
            );
            const normalizeSectionId = (section) => (section?.id === "top" ? "" : section?.id || "");

            const sectionsWithRects = trackedSections.map((section) => ({
                section,
                rect: section.getBoundingClientRect(),
            }));
            const currentSection = sectionsWithRects.find(({ rect }) => rect.top <= viewportMarker && rect.bottom > viewportMarker);

            if (currentSection) {
                return normalizeSectionId(currentSection.section);
            }

            const previousSection = [...sectionsWithRects]
                .reverse()
                .find(({ rect }) => rect.top <= viewportMarker);

            if (!previousSection) {
                return normalizeSectionId(sectionsWithRects[0]?.section);
            }

            return normalizeSectionId(previousSection.section);
        };

        const syncHashWithViewport = () => {
            updateLocationHash(getCurrentSectionId());
        };

        window.addEventListener("scroll", syncHashWithViewport, { passive: true });
        window.addEventListener("resize", syncHashWithViewport);
        window.addEventListener("load", syncHashWithViewport);
        syncHashWithViewport();
    }

    if (!particleField || prefersReducedMotion) {
        return;
    }

    const particleCount = 28;

    for (let i = 0; i < particleCount; i += 1) {
        const particle = document.createElement("span");
        particle.className = "particle";
        particle.style.left = `${Math.random() * 100}%`;
        particle.style.setProperty("--size", `${Math.random() * 3 + 2}px`);
        particle.style.setProperty("--duration", `${Math.random() * 10 + 12}s`);
        particle.style.setProperty("--delay", `${Math.random() * -18}s`);
        particle.style.setProperty("--drift", `${Math.random() * 120 - 60}px`);
        particleField.appendChild(particle);
    }
});
