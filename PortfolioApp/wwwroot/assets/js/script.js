document.addEventListener("DOMContentLoaded", () => {
    const prefersReducedMotion = window.matchMedia("(prefers-reduced-motion: reduce)").matches;
    const menuToggle = document.querySelector(".menu-toggle");
    const siteNav = document.querySelector("#site-nav");
    const scrollTopButton = document.querySelector(".scroll-top");

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
    if (heroName) {
        const fullText = heroName.dataset.text || "";
        heroName.textContent = "";

        if (prefersReducedMotion) {
            heroName.textContent = fullText;
            heroName.classList.add("is-complete");
        } else {
            heroName.classList.add("is-typing");

            let index = 0;
            const typeInterval = window.setInterval(() => {
                heroName.textContent += fullText.charAt(index);
                index += 1;

                if (index >= fullText.length) {
                    window.clearInterval(typeInterval);
                    heroName.classList.remove("is-typing");
                    heroName.classList.add("is-complete");
                }
            }, 55);
        }
    }

    const particleField = document.querySelector(".particle-field");
    if (scrollTopButton) {
        const toggleScrollButton = () => {
            scrollTopButton.classList.toggle("is-visible", window.scrollY > 320);
        };

        scrollTopButton.addEventListener("click", () => {
            window.scrollTo({
                top: 0,
                behavior: prefersReducedMotion ? "auto" : "smooth",
            });
        });

        window.addEventListener("scroll", toggleScrollButton, { passive: true });
        toggleScrollButton();
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
